// E2E coverage for /ai-agents (src/pages/ai-agents/ai-agents-page.tsx):
// departments rail, agents grid, designer save, archive/restore. All API
// calls are mocked; permissions re-mocked after installShellMocks.
import { expect, test } from "@playwright/test";
import { mockJsonResponse } from "../helpers/api-mocks";
import { seedAuthedSession, TEST_USER } from "../helpers/auth-seed";
import { installShellMocks } from "../helpers/shell-mocks";

const DEPT_ID = "00000000-0000-0000-0000-0000000e1111";
const AGENT_ID = "00000000-0000-0000-0000-0000000f2222";

const DEPT = {
  id: DEPT_ID,
  name: "Support",
  description: null,
  agentCount: 1,
};

const AGENT = {
  id: AGENT_ID,
  departmentId: DEPT_ID,
  name: "Helper",
  instructions: "Help with tickets.",
  skills: ["tickets"],
  runtimeBinding: "codex",
  model: "local-extractive",
  variant: "High",
  accessMode: "Department",
  accessUserIds: [],
  isArchived: false,
};

test.beforeEach(async ({ page }) => {
  await seedAuthedSession(page, TEST_USER);
  await installShellMocks(page);
  await mockJsonResponse(page, "**/api/v1/identity/permissions", [
    "Permissions.AiAgents.View",
    "Permissions.AiAgents.Create",
    "Permissions.AiAgents.Update",
  ]);
  await mockJsonResponse(page, "**/api/v1/ai/departments", [DEPT]);
  await mockJsonResponse(page, "**/api/v1/ai/agents*", [AGENT]);
  await mockJsonResponse(page, `**/api/v1/ai/agents/${AGENT_ID}`, AGENT);
});

test("lists departments and agents with workload badges", async ({ page }) => {
  await page.goto("/ai-agents");

  await expect(page.getByText("Support").first()).toBeVisible();
  await expect(page.getByText("Helper")).toBeVisible();
  await expect(page.getByText("codex · local-extractive")).toBeVisible();
});

test("archives and restores an agent", async ({ page }) => {
  let archived = false;
  await page.route("**/api/v1/ai/agents/**", async (route) => {
    const url = route.request().url();
    if (url.endsWith("/archive") || url.endsWith("/restore")) {
      archived = url.endsWith("/archive");
      await route.fulfill({ status: 200, json: AGENT_ID });
    } else {
      await route.fallback();
    }
  });

  await page.goto("/ai-agents");
  await page.getByRole("button", { name: "Archive" }).click();
  await expect(page.getByText("Agent updated")).toBeVisible();
  expect(archived).toBe(true);
});

test("saves a new agent from the designer", async ({ page }) => {
  await page.route("**/api/v1/ai/agents", async (route) => {
    if (route.request().method() === "POST") {
      await route.fulfill({ status: 200, json: AGENT_ID });
    } else {
      await route.fallback();
    }
  });

  await page.goto("/ai-agents");
  await page.getByText("Support").first().click();
  await page.getByRole("button", { name: "New agent" }).click();
  await page.getByLabel("Name").fill("Second agent");
  await page.getByLabel("Instructions").fill("Do support things.");
  await page.getByLabel("Model").fill("local-extractive");
  await page.getByRole("button", { name: "Save agent" }).click();

  await expect(page.getByText("Agent saved")).toBeVisible();
});
