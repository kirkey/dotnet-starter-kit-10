import { apiFetch } from "@/lib/api-client";
import { tokenStore } from "@/auth/token-store";
import { env } from "@/env";

export type AiChatSessionDto = {
  id: string;
  title: string;
  model: string;
  variant: string;
  agentId?: string | null;
  lastActivityUtc: string;
  messageCount: number;
};

export type AiChatMessageDto = {
  id: string;
  role: string;
  content: string;
  citedSources: string[];
};

export type AiChatSessionDetailDto = {
  session: AiChatSessionDto;
  messages: AiChatMessageDto[];
};

export type CreateAiChatSessionInput = {
  title?: string;
  model?: string;
  variant?: string;
  agentId?: string;
};

export type DetectedAgentDto = {
  family: string;
  displayName: string;
  detectedVersion?: string | null;
  hasCredentials: boolean;
};

export function listLocalAgents() {
  return apiFetch<DetectedAgentDto[]>("/api/v1/ai/detection/local-agents");
}

export function listAiChatSessions() {
  return apiFetch<AiChatSessionDto[]>("/api/v1/ai/sessions");
}

export function getAiChatSession(id: string) {
  return apiFetch<AiChatSessionDetailDto>(`/api/v1/ai/sessions/${id}`);
}

export function createAiChatSession(input: CreateAiChatSessionInput) {
  return apiFetch<string>("/api/v1/ai/sessions", {
    method: "POST",
    body: JSON.stringify(input),
  });
}

export function deleteAiChatSession(id: string) {
  return apiFetch<string>(`/api/v1/ai/sessions/${id}`, { method: "DELETE" });
}

export function sendAiChatMessage(sessionId: string, content: string) {
  return apiFetch<AiChatMessageDto>(`/api/v1/ai/sessions/${sessionId}/messages`, {
    method: "POST",
    body: JSON.stringify({ content }),
  });
}

export type AiChatStreamEvent =
  | { type: "token"; text: string }
  | { type: "done"; citedSources: string[] }
  | { type: "error"; message: string };

function streamHeaders() {
  const headers = new Headers({ "Content-Type": "application/json" });
  const accessToken = tokenStore.getAccessToken();
  if (accessToken) headers.set("Authorization", `Bearer ${accessToken}`);
  const tenant = tokenStore.getTenant() ?? env.defaultTenant;
  if (tenant) headers.set("tenant", tenant);
  return headers;
}

export async function* streamAiChatMessage(
  sessionId: string,
  content: string,
  signal?: AbortSignal,
): AsyncGenerator<AiChatStreamEvent> {
  const response = await fetch(`${env.apiBase}/api/v1/ai/sessions/${sessionId}/messages/stream`, {
    method: "POST",
    headers: streamHeaders(),
    body: JSON.stringify({ content }),
    signal,
  });
  if (!response.ok || !response.body) {
    throw new Error(`Chat stream failed with status ${response.status}`);
  }

  const reader = response.body.getReader();
  const decoder = new TextDecoder("utf-8");
  let buffer = "";
  for (;;) {
    const { value, done } = await reader.read();
    if (done) return;
    buffer += decoder.decode(value, { stream: true });
    let delimiter: number;
    while ((delimiter = buffer.indexOf("\n\n")) !== -1) {
      const rawEvent = buffer.slice(0, delimiter);
      buffer = buffer.slice(delimiter + 2);
      for (const line of rawEvent.split("\n")) {
        if (!line.startsWith("data:")) continue;
        const payload = JSON.parse(line.slice(5).trim()) as AiChatStreamEvent;
        yield payload;
        if (payload.type === "done" || payload.type === "error") return;
      }
    }
  }
}
