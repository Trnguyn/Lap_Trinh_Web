export default function Navbar() {
  return (
    <header className="sticky top-0 z-50 backdrop-blur bg-black/40 ring-1 ring-white/10">
      <div className="mx-auto max-w-6xl px-4 h-14 flex items-center justify-between">
        <div className="text-white font-semibold">JHF Store</div>
        <div className="text-white/60 text-sm">Catalog demo</div>
      </div>
    </header>
  );
}
