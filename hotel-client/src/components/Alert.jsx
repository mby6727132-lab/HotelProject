const ICONS = { error: "⚠", success: "✓", warning: "ℹ" };

export default function Alert({ type = "error", children }) {
  if (!children) return null;
  return (
    <div className={`alert alert-${type}`} role={type === "error" ? "alert" : "status"}>
      <span aria-hidden="true">{ICONS[type]}</span>
      <span>{children}</span>
    </div>
  );
}
