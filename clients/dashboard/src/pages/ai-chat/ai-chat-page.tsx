import { useState } from "react";
import { keepPreviousData, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { MessageCircle, Sparkles } from "lucide-react";
import { toast } from "sonner";
import { ApiRequestError } from "@/lib/api-client";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Skeleton } from "@/components/ui/skeleton";
import { EntityEmpty, EntityListLoading, ErrorBand, PageHero } from "@/components/list";
import {
  createAiChatSession,
  deleteAiChatSession,
  getAiChatSession,
  listAiChatSessions,
  listLocalAgents,
  streamAiChatMessage,
  type AiChatMessageDto,
} from "@/api/ai-chat";

const VARIANTS = ["Default", "Normal", "High", "ExtraHigh"] as const;

function errMessage(e: unknown, fallback: string) {
  return e instanceof ApiRequestError ? e.message : fallback;
}

export function AiChatPage() {
  const qc = useQueryClient();
  const [activeId, setActiveId] = useState<string | null>(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [title, setTitle] = useState("");
  const [model, setModel] = useState("");
  const [variant, setVariant] = useState<string>("Default");
  const [target, setTarget] = useState<string>("default");
  const [draft, setDraft] = useState("");
  const [streaming, setStreaming] = useState(false);
  const [streamText, setStreamText] = useState("");

  const sessionsQuery = useQuery({
    queryKey: ["ai-chat-sessions"],
    queryFn: listAiChatSessions,
    placeholderData: keepPreviousData,
  });

  const activeQuery = useQuery({
    queryKey: ["ai-chat-session", { id: activeId }],
    queryFn: () => getAiChatSession(activeId ?? ""),
    enabled: activeId !== null,
    placeholderData: keepPreviousData,
  });

  const createMut = useMutation({
    mutationFn: (input: { title?: string; model?: string; variant: string; agentId?: string }) =>
      createAiChatSession({
        title: input.title || undefined,
        model: input.model || undefined,
        variant: input.variant,
        agentId: input.agentId,
      }),
    onSuccess: (id) => {
      toast.success("Chat started");
      setTitle("");
      setModel("");
      setVariant("Default");
      setTarget("default");
      setDialogOpen(false);
      qc.invalidateQueries({ queryKey: ["ai-chat-sessions"] });
      setActiveId(id);
    },
    onError: (e) => toast.error(errMessage(e, "Could not start chat")),
  });

  const deleteMut = useMutation({
    mutationFn: (id: string) => deleteAiChatSession(id),
    onSuccess: (_id, id) => {
      toast.success("Chat deleted");
      if (activeId === id) setActiveId(null);
      qc.invalidateQueries({ queryKey: ["ai-chat-sessions"] });
    },
    onError: (e) => toast.error(errMessage(e, "Could not delete chat")),
  });

  async function handleSend() {
    const content = draft.trim();
    if (!content || !activeId || streaming) return;
    setDraft("");
    setStreaming(true);
    setStreamText("");
    try {
      let done = false;
      for await (const ev of streamAiChatMessage(activeId, content)) {
        if (ev.type === "token") {
          setStreamText((prev) => prev + ev.text);
        } else if (ev.type === "done") {
          done = true;
        } else {
          toast.error(ev.message || "Chat failed");
        }
      }
      if (!done) toast.error("Stream ended unexpectedly");
    } catch (e) {
      toast.error(errMessage(e, "Could not send message"));
    } finally {
      setStreaming(false);
      setStreamText("");
      qc.invalidateQueries({ queryKey: ["ai-chat-session", { id: activeId }] });
      qc.invalidateQueries({ queryKey: ["ai-chat-sessions"] });
    }
  }

  const sessions = sessionsQuery.data ?? [];
  const messages: AiChatMessageDto[] = activeQuery.data?.messages ?? [];
  const showStreaming = streaming && streamText.length > 0;

  const agentsQuery = useQuery({
    queryKey: ["ai-local-agents"],
    queryFn: listLocalAgents,
    placeholderData: keepPreviousData,
  });
  const agents = agentsQuery.data ?? [];

  function createPayload() {
    // Detected CLIs are shown for availability (spec) but cannot execute
    // server-side; they run on the tenant default model until bound to a
    // department agent. Only department agents (GUIDs) bind runs.
    if (target.startsWith("agent:") || target === "default") {
      return { title, variant };
    }
    return { title, model, variant };
  }

  return (
    <div className="space-y-4">
      <PageHero
        eyebrow="Knowledge"
        title="AI Chat"
        subtitle="Ask questions grounded in your knowledge sources."
        actions={
          <Dialog open={dialogOpen} onOpenChange={setDialogOpen}>
            <DialogTrigger asChild>
              <Button>New chat</Button>
            </DialogTrigger>
            <DialogContent>
              <DialogHeader>
                <DialogTitle>Start a chat</DialogTitle>
              </DialogHeader>
              <div className="space-y-3">
                <div className="space-y-1">
                  <Label htmlFor="ai-chat-title">Title</Label>
                  <Input
                    id="ai-chat-title"
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                    placeholder="Optional title"
                  />
                </div>
                <div className="space-y-1">
                  <Label htmlFor="ai-chat-target">Answer target</Label>
                  <select
                    id="ai-chat-target"
                    className="w-full rounded-md border px-2 py-2 text-sm"
                    value={target}
                    onChange={(e) => setTarget(e.target.value)}
                  >
                    <option value="default">Tenant default model</option>
                    <option value="model">Custom model…</option>
                    {agents.map((a) => (
                      <option key={a.family} value={`agent:${a.family}`}>
                        {a.displayName} (detected
                        {a.hasCredentials ? ", credentials present" : ", no credentials"}
                        )
                      </option>
                    ))}
                  </select>
                </div>
                {target === "model" && (
                  <div className="space-y-1">
                    <Label htmlFor="ai-chat-model">Model target</Label>
                    <Input
                      id="ai-chat-model"
                      value={model}
                      onChange={(e) => setModel(e.target.value)}
                      placeholder="Model id served by a provider"
                    />
                  </div>
                )}
                <div className="space-y-1">
                  <Label htmlFor="ai-chat-variant">Variant</Label>
                  <select
                    id="ai-chat-variant"
                    className="w-full rounded-md border px-2 py-2 text-sm"
                    value={variant}
                    onChange={(e) => setVariant(e.target.value)}
                  >
                    {VARIANTS.map((v) => (
                      <option key={v} value={v}>
                        {v}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
              <DialogFooter>
                <Button
                  disabled={createMut.isPending}
                  onClick={() => createMut.mutate(createPayload())}
                >
                  {createMut.isPending ? "Starting…" : "Start chat"}
                </Button>
              </DialogFooter>
            </DialogContent>
          </Dialog>
        }
      />

      {sessionsQuery.isError && <ErrorBand message="Could not load chats" />}

      <div className="grid gap-4 md:grid-cols-[280px_1fr]">
        <Card className="p-2">
          {sessionsQuery.isPending ? (
            <EntityListLoading />
          ) : sessions.length === 0 ? (
            <EntityEmpty
              icon={MessageCircle}
              title="No chats yet"
              body="Start a chat to ask your sources."
            />
          ) : (
            <ul className="space-y-1">
              {sessions.map((s) => (
                <li key={s.id}>
                  <div
                    role="button"
                    tabIndex={0}
                    onClick={() => setActiveId(s.id)}
                    onKeyDown={(e) => e.key === "Enter" && setActiveId(s.id)}
                    className={`w-full rounded-md px-2 py-2 text-left text-sm hover:bg-muted ${
                      activeId === s.id ? "bg-muted" : ""
                    }`}
                  >
                    <div className="flex items-center justify-between gap-2">
                      <span className="truncate font-medium">{s.title}</span>
                      <Button
                        variant="ghost"
                        size="sm"
                        aria-label={`Delete ${s.title}`}
                        disabled={deleteMut.isPending}
                        onClick={(e) => {
                          e.stopPropagation();
                          deleteMut.mutate(s.id);
                        }}
                      >
                        ✕
                      </Button>
                    </div>
                    <div className="mt-1 flex items-center gap-2 text-xs text-muted-foreground">
                      <Badge variant="outline">{s.model}</Badge>
                      <span>{s.messageCount} messages</span>
                    </div>
                  </div>
                </li>
              ))}
            </ul>
          )}
        </Card>

        <Card className="flex min-h-[420px] flex-col p-4">
          {!activeId ? (
            <EntityEmpty
              icon={Sparkles}
              title="Select a chat"
              body="Pick a session on the left or start a new one."
            />
          ) : activeQuery.isPending ? (
            <div className="space-y-2">
              <Skeleton className="h-10 w-3/4" />
              <Skeleton className="h-10 w-2/3" />
              <Skeleton className="h-10 w-1/2" />
            </div>
          ) : activeQuery.isError ? (
            <ErrorBand message="Could not load this chat" />
          ) : (
            <>
              <div className="mb-2 flex items-center gap-2">
                <h2 className="truncate text-base font-semibold">
                  {activeQuery.data?.session.title}
                </h2>
                <Badge variant="outline">{activeQuery.data?.session.model}</Badge>
                <Badge variant="secondary">{activeQuery.data?.session.variant}</Badge>
              </div>
              <div className="flex-1 space-y-3 overflow-y-auto">
                {messages.map((m) => (
                  <div
                    key={m.id}
                    className={`max-w-[85%] rounded-lg px-3 py-2 text-sm ${
                      m.role === "user"
                        ? "ml-auto bg-primary text-primary-foreground"
                        : "bg-muted"
                    }`}
                  >
                    <p className="whitespace-pre-wrap">{m.content}</p>
                    {m.citedSources.length > 0 && (
                      <p className="mt-1 text-xs opacity-80">
                        Sources: {m.citedSources.join(", ")}
                      </p>
                    )}
                  </div>
                ))}
                {showStreaming && (
                  <div className="max-w-[85%] rounded-lg bg-muted px-3 py-2 text-sm">
                    <p className="whitespace-pre-wrap">{streamText}</p>
                  </div>
                )}
                {streaming && !showStreaming && (
                  <p className="text-sm text-muted-foreground">Thinking…</p>
                )}
              </div>
              <div className="mt-3 flex gap-2">
                <Input
                  aria-label="Chat message"
                  value={draft}
                  onChange={(e) => setDraft(e.target.value)}
                  placeholder="Ask your sources…"
                  disabled={streaming}
                  onKeyDown={(e) => {
                    if (e.key === "Enter" && !e.shiftKey) {
                      e.preventDefault();
                      void handleSend();
                    }
                  }}
                />
                <Button disabled={streaming || draft.trim().length === 0} onClick={() => void handleSend()}>
                  {streaming ? "Sending…" : "Send"}
                </Button>
              </div>
            </>
          )}
        </Card>
      </div>
    </div>
  );
}
