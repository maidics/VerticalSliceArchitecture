import { Toaster as Sonner } from "sonner";

type ToasterProps = React.ComponentProps<typeof Sonner>;

const Toaster = ({ ...props }: ToasterProps) => {
  return (
    <Sonner
      theme="dark"
      richColors
      closeButton
      position="bottom-right"
      toastOptions={{
        style: {
          background: "hsl(0 0% 5% / 0.95)",
          border: "1px solid hsl(135 100% 50% / 0.35)",
          color: "hsl(135 100% 75%)",
          fontFamily: '"JetBrains Mono", monospace',
          backdropFilter: "blur(10px)",
        },
      }}
      {...props}
    />
  );
};

export { Toaster };
