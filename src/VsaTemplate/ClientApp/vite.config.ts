import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";

const target =
  process.env["services__webapi__https__0"] ||
  process.env["services__webapi__http__0"];

const proxyOptions = target
  ? { target, secure: false, changeOrigin: true }
  : undefined;

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
    port: parseInt(process.env.PORT!),
    proxy: proxyOptions
      ? {
          "/api": proxyOptions,
          "/openapi": proxyOptions,
          "/scalar": proxyOptions,
        }
      : undefined,
  },
  build: {
    outDir: "build",
  },
});
