import { createBrowserRouter } from "react-router";

const baseUrl =
  document.getElementsByTagName("base")[0]?.getAttribute("href") || "/";

export const router = createBrowserRouter(
  [
    {
      path: "/",
      element: undefined,
      children: [],
    },
  ],
  { basename: baseUrl },
);
