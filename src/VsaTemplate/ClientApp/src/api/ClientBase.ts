export class ClientBase {
  protected getBaseUrl(_defaultUrl: string, baseUrl?: string) {
    return baseUrl !== undefined && baseUrl !== null
      ? baseUrl
      : (import.meta.env.VITE_API_URL ?? "");
  }
}
