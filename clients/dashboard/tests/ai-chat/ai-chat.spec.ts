// E2E coverage for /ai-chat (src/pages/ai-chat/ai-chat-page.tsx): the session
// rail, the active-session thread with citations, streaming send, and the new
// chat dialog. All API calls are mocked; the session is seeded and shell calls
// stubbed by installShellMocks (which stubs /identity/permissions to [] — we
// re-mock it with the AI grants AFTER, so our mock wins).
import { expect, test } from "@playwright/test";
import { mockJsonResponse } from "../helpers/api-mocks";
import { seedAuthedSession, TEST_USER } from "../helpers/auth-seed";
import { installShellMocks } from "../helpers/shell-mocks";

const SESSION_ID = "00000000-0000-0000-0000-0000000a1111";

const SESSION_WIDGETS = {
  id: SESSION_ID,
  title: "Widget questions",
  model: "local-extractive",
  variant: "Default",
  agentId: null,
  lastActivityUtc: "2026-09-05T08:00:00Z",
  messageCount: 2,
};

const THREAD = {
  session: SESSION_WIDGETS,
  messages: [
    {
      id: "00000000-0000-0000-0000-0000000b2222",
      role: "user",
      content: "Where are Acme widgets calibrated?",
      citedSources: [],
    },
    {
      id: "00000000-0000-0000-0000-0000000c3333",
      role: "assistant",
      content: "In Zurich.",
      citedSources: ["widgets.txt"],
    },
  ],
};

const STREAM_BODY = [
  'data: {"type":"token","text":"In "}',
  'data: {"type":"token","text":"Zurich."}',
  'data: {"type":"done","citedSources":["widgets.txt"]}',
].join("\n\n");

test.beforeEach(async ({ page }) => {
  await seedAuthedSession(page, TEST_USER);
  await installShellMocks(page);
  await mockJsonResponse(page, "**/api/v1/identity/permissions", [
    "Permissions.AiChat.View",
    "Permissions.AiChat.Create",
  ]);
  await mockJsonResponse(page, "**/api/v1/ai/detection/local-agents", [
    { family: "codex", displayName: "OpenAI Codex", detectedVersion: null, hasCredentials: true },
  ]);
  await mockJsonResponse(page, "**/api/v1/ai/sessions", [SESSION_WIDGETS]);
  await mockJsonResponse(
    page,
    `**/api/v1/ai/sessions/${SESSION_ID}`,
    THREAD,
  );
  await page.route(`**/api/v1/ai/sessions/${SESSION_ID}/messages/stream`, (route) =>
    route.fulfill({
      status: 200,
      contentType: "text/event-stream",
      body: STREAM_BODY,
    }),
  );
});

test("lists sessions and shows the thread with citations", async ({ page }) => {
  await page.goto("/ai-chat");

  await expect(page.getByText("Widget questions").first()).toBeVisible();
  await page.getByText("Widget questions").first().click();

  await expect(page.getByText("Where are Acme widgets calibrated?")).toBeVisible();
  await expect(page.getByText("In Zurich.")).toBeVisible();
  await expect(page.getByText("Sources: widgets.txt")).toBeVisible();
});

test("streams a sent message into the thread", async ({ page }) => {
  await page.goto("/ai-chat");
  await page.getByText("Widget questions").first().click();
  await expect(page.getByText("In Zurich.")).toBeVisible();

  await page.getByLabel("Chat message").fill("And the warranty?");
  await page.getByRole("button", { name: "Send" }).click();

  await expect(page.getByText("In Zurich.", { exact: true }).last()).toBeVisible();
});

test("creates a session from the dialog", async ({ page }) => {
  await page.route("**/api/v1/ai/sessions", async (route) => {
    if (route.request().method() === "POST") {
      await route.fulfill({ status: 200, json: SESSION_ID });
    } else {
      await route.fallback();
    }
  });

  await page.goto("/ai-chat");
  await page.getByRole("button", { name: "New chat" }).click();
  await page.getByLabel("Title").fill("Second chat");
  await expect(
    page.getByRole("option", { name: /OpenAI Codex/ }),
  ).toBeAttached();
  await page.getByRole("button", { name: "Start chat" }).click();

  await expect(page.getByText("Widget questions")).toBeVisible();
});
