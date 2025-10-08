import React from "react";
import clsx from "clsx";

export function Card({
  children,
  className,
}: React.PropsWithChildren<{ className?: string }>) {
  return (
    <div className={clsx("rounded-2xl bg-white/5 ring-1 ring-white/10 overflow-hidden", className)}>
      {children}
    </div>
  );
}

export function CardBody({
  children,
  className,
}: React.PropsWithChildren<{ className?: string }>) {
  return <div className={clsx("p-5", className)}>{children}</div>;
}
