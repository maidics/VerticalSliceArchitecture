import { Component } from "react";
import { QueryClientProvider } from "@tanstack/react-query";
import { RouterProvider } from "react-router-dom";
import { router } from "./routes.tsx";
import { queryClient } from "./api/queryClient.ts";
import { TooltipProvider } from "./common/components/ui/tooltip.tsx";
import { Toaster } from "./common/components/ui/toaster.tsx";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <QueryClientProvider client={queryClient}>
        <TooltipProvider>
          <Toaster />

          <RouterProvider router={router} />
        </TooltipProvider>
      </QueryClientProvider>
    );
  }
}
