import { useState } from "react";
import { keepPreviousData, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Bot } from "lucide-react";
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
import { EntityEmpty, EntityListLoading, ErrorBand, PageHero } from "@/components/list";
import {
  AI_RUNTIME_FAMILIES,
  AI_VARIANTS,
  archiveAiAgent,
  copyAiAgent,
  createAiAgent,
  createAiDepartment,
  deleteAiDepartment,
  getAiAgent,
  listAiAgents,
  listAiDepartments,
  restoreAiAgent,
  updateAiAgent,
  type AiAgentDto,
} from "@/api/ai-agents";

function errMessage(e: unknown, fallback: string) {
  return e instanceof ApiRequestError ? e.message : fallback;
}

type DesignerState = {
  id?: string;
  departmentId: string;
  name: string;
  instructions: string;
  skills: string;
  runtimeBinding: string;
  model: string;
  variant: string;
  accessMode: string;
  accessUserIds: string;
};

const emptyDesigner = (departmentId: string): DesignerState => ({
  departmentId,
  name: "",
  instructions: "",
  skills: "",
  runtimeBinding: "local-model",
  model: "local-extractive",
  variant: "Default",
  accessMode: "Department",
  accessUserIds: "",
});

function splitLines(v: string) {
  return v.split("\n").map((s) => s.trim()).filter((s) => s.length > 0);
}

export function AiAgentsPage() {
  const qc = useQueryClient();
  const [deptId, setDeptId] = useState<string | null>(null);
  const [showArchived, setShowArchived] = useState(false);
  const [deptDialog, setDeptDialog] = useState(false);
  const [deptName, setDeptName] = useState("");
  const [designerOpen, setDesignerOpen] = useState(false);
  const [designer, setDesigner] = useState<DesignerState>(emptyDesigner(""));

  const deptsQuery = useQuery({
    queryKey: ["ai-departments"],
    queryFn: listAiDepartments,
    placeholderData: keepPreviousData,
  });

  const agentsQuery = useQuery({
    queryKey: ["ai-agents", { deptId, showArchived }],
    queryFn: () => listAiAgents(deptId ?? undefined, showArchived),
    placeholderData: keepPreviousData,
  });

  const invalidateAll = () => {
    qc.invalidateQueries({ queryKey: ["ai-departments"] });
    qc.invalidateQueries({ queryKey: ["ai-agents"] });
  };

  const deptMut = useMutation({
    mutationFn: (name: string) => createAiDepartment({ name }),
    onSuccess: () => {
      toast.success("Department created");
      setDeptName("");
      setDeptDialog(false);
      qc.invalidateQueries({ queryKey: ["ai-departments"] });
    },
    onError: (e) => toast.error(errMessage(e, "Could not create department")),
  });

  const delDeptMut = useMutation({
    mutationFn: (id: string) => deleteAiDepartment(id),
    onSuccess: (_r, id) => {
      toast.success("Department deleted");
      if (deptId === id) setDeptId(null);
      qc.invalidateQueries({ queryKey: ["ai-departments"] });
    },
    onError: (e) => toast.error(errMessage(e, "Could not delete department")),
  });

  const saveMut = useMutation({
    mutationFn: (d: DesignerState) => {
      const payload = {
        departmentId: d.departmentId,
        name: d.name,
        instructions: d.instructions,
        skills: splitLines(d.skills),
        runtimeBinding: d.runtimeBinding,
        model: d.model,
        variant: d.variant,
        accessMode: d.accessMode,
        accessUserIds: splitLines(d.accessUserIds),
      };
      return d.id ? updateAiAgent(d.id, payload) : createAiAgent(payload);
    },
    onSuccess: () => {
      toast.success("Agent saved");
      setDesignerOpen(false);
      invalidateAll();
    },
    onError: (e) => toast.error(errMessage(e, "Could not save agent")),
  });

  const archiveMut = useMutation({
    mutationFn: (a: { id: string; archived: boolean }) =>
      a.archived ? restoreAiAgent(a.id) : archiveAiAgent(a.id),
    onSuccess: () => {
      toast.success("Agent updated");
      invalidateAll();
    },
    onError: (e) => toast.error(errMessage(e, "Could not update agent")),
  });

  const copyMut = useMutation({
    mutationFn: (a: AiAgentDto) => copyAiAgent(a.id, `${a.name} (copy)`),
    onSuccess: () => {
      toast.success("Agent copied");
      invalidateAll();
    },
    onError: (e) => toast.error(errMessage(e, "Could not copy agent")),
  });

  async function openDesigner(agentId?: string, forDept?: string) {
    if (agentId) {
      try {
        const a = await getAiAgent(agentId);
        setDesigner({
          id: a.id,
          departmentId: a.departmentId,
          name: a.name,
          instructions: a.instructions,
          skills: a.skills.join("\n"),
          runtimeBinding: a.runtimeBinding,
          model: a.model,
          variant: a.variant,
          accessMode: a.accessMode,
          accessUserIds: a.accessUserIds.join("\n"),
        });
      } catch (e) {
        toast.error(errMessage(e, "Could not load agent"));
        return;
      }
    } else {
      setDesigner(emptyDesigner(forDept ?? deptId ?? ""));
    }
    setDesignerOpen(true);
  }

  const depts = deptsQuery.data ?? [];
  const agents = agentsQuery.data ?? [];

  return (
    <div className="space-y-4">
      <PageHero
        eyebrow="Knowledge"
        title="AI Agents"
        subtitle="Department agents with models, variants, and access control."
        actions={
          <Dialog open={deptDialog} onOpenChange={setDeptDialog}>
            <DialogTrigger asChild>
              <Button variant="outline">New department</Button>
            </DialogTrigger>
            <DialogContent>
              <DialogHeader>
                <DialogTitle>New department</DialogTitle>
              </DialogHeader>
              <div className="space-y-1">
                <Label htmlFor="ai-dept-name">Name</Label>
                <Input
                  id="ai-dept-name"
                  value={deptName}
                  onChange={(e) => setDeptName(e.target.value)}
                  placeholder="Support"
                />
              </div>
              <DialogFooter>
                <Button disabled={deptMut.isPending || deptName.trim().length === 0} onClick={() => deptMut.mutate(deptName.trim())}>
                  {deptMut.isPending ? "Creating…" : "Create"}
                </Button>
              </DialogFooter>
            </DialogContent>
          </Dialog>
        }
      />

      {deptsQuery.isError && <ErrorBand message="Could not load departments" />}

      <div className="grid gap-4 md:grid-cols-[240px_1fr]">
        <Card className="p-2">
          <p className="px-2 py-1 text-xs font-semibold uppercase text-muted-foreground">Departments</p>
          {deptsQuery.isPending ? (
            <EntityListLoading />
          ) : (
            <ul className="space-y-1">
              <li>
                <button
                  type="button"
                  onClick={() => setDeptId(null)}
                  className={`w-full rounded-md px-2 py-2 text-left text-sm hover:bg-muted ${deptId === null ? "bg-muted" : ""}`}
                >
                  All departments
                </button>
              </li>
              {depts.map((d) => (
                <li key={d.id}>
                  <div
                    role="button"
                    tabIndex={0}
                    onClick={() => setDeptId(d.id)}
                    onKeyDown={(e) => e.key === "Enter" && setDeptId(d.id)}
                    className={`w-full rounded-md px-2 py-2 text-left text-sm hover:bg-muted ${deptId === d.id ? "bg-muted" : ""}`}
                  >
                    <div className="flex items-center justify-between gap-2">
                      <span className="truncate font-medium">{d.name}</span>
                      <Button
                        variant="ghost"
                        size="sm"
                        aria-label={`Delete ${d.name}`}
                        onClick={(e) => {
                          e.stopPropagation();
                          delDeptMut.mutate(d.id);
                        }}
                      >
                        ✕
                      </Button>
                    </div>
                    <p className="text-xs text-muted-foreground">{d.agentCount} agents</p>
                  </div>
                </li>
              ))}
            </ul>
          )}
        </Card>

        <div className="space-y-3">
          <div className="flex items-center justify-between">
            <label className="flex items-center gap-2 text-sm">
              <input
                type="checkbox"
                checked={showArchived}
                onChange={(e) => setShowArchived(e.target.checked)}
              />
              Show archived
            </label>
            <Button disabled={!deptId && depts.length > 0} onClick={() => void openDesigner(undefined, deptId ?? depts[0]?.id)}>
              New agent
            </Button>
          </div>

          {agentsQuery.isError && <ErrorBand message="Could not load agents" />}
          {agentsQuery.isPending ? (
            <EntityListLoading />
          ) : agents.length === 0 ? (
            <Card className="p-2">
              <EntityEmpty icon={Bot} title="No agents" body="Create an agent to get started." />
            </Card>
          ) : (
            <ul className="grid gap-3 md:grid-cols-2">
              {agents.map((a) => (
                <li key={a.id}>
                  <Card className="p-4">
                    <div className="flex items-start justify-between gap-2">
                      <div>
                        <p className="font-semibold">{a.name}</p>
                        <p className="text-xs text-muted-foreground">{a.runtimeBinding} · {a.model}</p>
                      </div>
                      <div className="flex gap-1">
                        <Badge variant="outline">{a.variant}</Badge>
                        {a.isArchived && <Badge variant="secondary">archived</Badge>}
                      </div>
                    </div>
                    <p className="mt-2 line-clamp-2 text-sm text-muted-foreground">{a.instructions}</p>
                    <div className="mt-3 flex flex-wrap gap-2">
                      <Button variant="outline" size="sm" onClick={() => void openDesigner(a.id)}>
                        Edit
                      </Button>
                      <Button variant="outline" size="sm" onClick={() => copyMut.mutate(a)}>
                        Copy
                      </Button>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => archiveMut.mutate({ id: a.id, archived: a.isArchived })}
                      >
                        {a.isArchived ? "Restore" : "Archive"}
                      </Button>
                    </div>
                  </Card>
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>

      <Dialog open={designerOpen} onOpenChange={setDesignerOpen}>
        <DialogContent className="max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>{designer.id ? "Edit agent" : "New agent"}</DialogTitle>
          </DialogHeader>
          <div className="space-y-3">
            <div className="space-y-1">
              <Label htmlFor="ai-agent-name">Name</Label>
              <Input id="ai-agent-name" value={designer.name} onChange={(e) => setDesigner({ ...designer, name: e.target.value })} />
            </div>
            <div className="space-y-1">
              <Label htmlFor="ai-agent-instructions">Instructions</Label>
              <textarea
                id="ai-agent-instructions"
                className="min-h-24 w-full rounded-md border px-2 py-2 text-sm"
                value={designer.instructions}
                onChange={(e) => setDesigner({ ...designer, instructions: e.target.value })}
              />
            </div>
            <div className="grid grid-cols-2 gap-3">
              <div className="space-y-1">
                <Label htmlFor="ai-agent-runtime">Runtime</Label>
                <Input
                  id="ai-agent-runtime"
                  list="ai-runtime-families"
                  value={designer.runtimeBinding}
                  onChange={(e) => setDesigner({ ...designer, runtimeBinding: e.target.value })}
                />
                <datalist id="ai-runtime-families">
                  {AI_RUNTIME_FAMILIES.map((f) => (
                    <option key={f} value={f} />
                  ))}
                </datalist>
              </div>
              <div className="space-y-1">
                <Label htmlFor="ai-agent-variant">Variant</Label>
                <select
                  id="ai-agent-variant"
                  className="w-full rounded-md border px-2 py-2 text-sm"
                  value={designer.variant}
                  onChange={(e) => setDesigner({ ...designer, variant: e.target.value })}
                >
                  {AI_VARIANTS.map((v) => (
                    <option key={v} value={v}>
                      {v}
                    </option>
                  ))}
                </select>
              </div>
            </div>
            <div className="space-y-1">
              <Label htmlFor="ai-agent-model">Model</Label>
              <Input id="ai-agent-model" value={designer.model} onChange={(e) => setDesigner({ ...designer, model: e.target.value })} />
            </div>
            <div className="space-y-1">
              <Label htmlFor="ai-agent-skills">Skills (one per line)</Label>
              <textarea
                id="ai-agent-skills"
                className="min-h-16 w-full rounded-md border px-2 py-2 text-sm"
                value={designer.skills}
                onChange={(e) => setDesigner({ ...designer, skills: e.target.value })}
              />
            </div>
            <div className="grid grid-cols-2 gap-3">
              <div className="space-y-1">
                <Label htmlFor="ai-agent-access">Access</Label>
                <select
                  id="ai-agent-access"
                  className="w-full rounded-md border px-2 py-2 text-sm"
                  value={designer.accessMode}
                  onChange={(e) => setDesigner({ ...designer, accessMode: e.target.value })}
                >
                  <option value="Department">Department</option>
                  <option value="Selected">Selected users</option>
                </select>
              </div>
              {designer.accessMode === "Selected" && (
                <div className="space-y-1">
                  <Label htmlFor="ai-agent-users">User ids (one per line)</Label>
                  <textarea
                    id="ai-agent-users"
                    className="min-h-16 w-full rounded-md border px-2 py-2 text-sm"
                    value={designer.accessUserIds}
                    onChange={(e) => setDesigner({ ...designer, accessUserIds: e.target.value })}
                  />
                </div>
              )}
            </div>
          </div>
          <DialogFooter>
            <Button disabled={saveMut.isPending} onClick={() => saveMut.mutate(designer)}>
              {saveMut.isPending ? "Saving…" : "Save agent"}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}
