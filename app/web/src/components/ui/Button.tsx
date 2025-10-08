import React from "react";
import clsx from "clsx";

type Props = React.ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: "primary" | "ghost" | "outline";
  size?: "sm" | "md" | "lg";
};

export default function Button({ className, variant = "primary", size = "md", ...props }: Props) {
  const base = "inline-flex items-center justify-center rounded-2xl font-medium transition";
  const variants = {
    primary: "bg-blue-600 text-white hover:bg-blue-500",
    ghost: "bg-transparent hover:bg-white/5 text-white",
    outline: "border border-white/15 hover:bg-white/5 text-white",
  };
  const sizes = {
    sm: "h-9 px-3 text-sm",
    md: "h-10 px-4",
    lg: "h-12 px-5 text-base",
  };
  return <button className={clsx(base, variants[variant], sizes[size], className)} {...props} />;
}
