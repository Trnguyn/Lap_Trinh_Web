import React from "react";
import clsx from "clsx";

export default function Badge({
  children,
  color = "blue",
  className,
}: React.PropsWithChildren<{ color?: "blue" | "red" | "gray"; className?: string }>) {
  const colorMap = {
    blue: "bg-blue-600/20 text-blue-300 ring-1 ring-inset ring-blue-600/30",
    red: "bg-red-600/20 text-red-300 ring-1 ring-inset ring-red-600/30",
    gray: "bg-white/10 text-white/70 ring-1 ring-inset ring-white/15",
  };
  return (
    <span className={clsx("px-2 py-0.5 rounded-full text-xs", colorMap[color], className)}>
      {children}
    </span>
  );
}
