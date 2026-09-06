import { apiFetch } from "@/lib/api-client";

export type AiDepartmentDto = {
  id: string;
  name: string;
  description?: string | null;
  agentCount: number;
};

export type AiAgentDto = {
  id: string;
  departmentId: string;
  name: string;
  instructions: string;
  skills: string[];
  runtimeBinding: string;
  model: string;
  variant: string;
  accessMode: string;
  accessUserIds: string[];
  isArchived: boolean;
};

export type AiAgentInput = {
  departmentId: string;
  name: string;
  instructions: string;
  skills: string[];
  runtimeBinding: string;
  model: string;
  variant: string;
  accessMode: string;
  accessUserIds: string[];
};

export const AI_RUNTIME_FAMILIES = [
  "claude-code",
  "opencode",
  "codex",
  "cursor",
  "gemini-cli",
  "local-model",
];

export const AI_VARIANTS = ["Default", "Normal", "High", "ExtraHigh"];

export function listAiDepartments() {
  return apiFetch<AiDepartmentDto[]>("/api/v1/ai/departments");
}

export function createAiDepartment(input: { name: string; description?: string }) {
  return apiFetch<string>("/api/v1/ai/departments", {
    method: "POST",
    body: JSON.stringify(input),
  });
}

export function deleteAiDepartment(id: string) {
  return apiFetch<string>(`/api/v1/ai/departments/${id}`, { method: "DELETE" });
}

export function listAiAgents(departmentId?: string, includeArchived = false) {
  const q = new URLSearchParams();
  if (departmentId) q.set("departmentId", departmentId);
  if (includeArchived) q.set("includeArchived", "true");
  const suffix = q.toString() ? `?${q}` : "";
  return apiFetch<AiAgentDto[]>(`/api/v1/ai/agents${suffix}`);
}

export function getAiAgent(id: string) {
  return apiFetch<AiAgentDto>(`/api/v1/ai/agents/${id}`);
}

export function createAiAgent(input: AiAgentInput) {
  return apiFetch<string>("/api/v1/ai/agents", {
    method: "POST",
    body: JSON.stringify(input),
  });
}

export function updateAiAgent(id: string, input: AiAgentInput) {
  return apiFetch<string>(`/api/v1/ai/agents/${id}`, {
    method: "PUT",
    body: JSON.stringify({ ...input, id }),
  });
}

export function archiveAiAgent(id: string) {
  return apiFetch<string>(`/api/v1/ai/agents/${id}/archive`, { method: "POST" });
}

export function restoreAiAgent(id: string) {
  return apiFetch<string>(`/api/v1/ai/agents/${id}/restore`, { method: "POST" });
}

export function copyAiAgent(id: string, name: string) {
  return apiFetch<string>(`/api/v1/ai/agents/${id}/duplicate`, {
    method: "POST",
    body: JSON.stringify({ name }),
  });
}
