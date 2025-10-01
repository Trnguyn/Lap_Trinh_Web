import Image from "next/image";

type ProductItem = {
  id: number;
  name: string;
  slug: string;
  brand: string;
  imageUrl?: string | null;
  price: number;
  isOnSale: boolean;
  preorderStatus: string;
};

async function getProducts() {
  const base = process.env.NEXT_PUBLIC_API_BASE!;
  const res = await fetch(`${base}/api/products?page=1&pageSize=12`, {
    headers: { accept: "application/json" },
    cache: "no-store", // tránh cache khi dev
  });
  if (!res.ok) throw new Error("Failed to fetch products");
  return (await res.json()) as { total: number; items: ProductItem[] };
}

export default async function Home() {
  const data = await getProducts();

  return (
    <main className="max-w-6xl mx-auto px-4 py-8">
      <h1 className="text-2xl font-semibold mb-6">Catalog</h1>

      <ul className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-5">
        {data.items.map((p) => (
          <li
            key={p.id}
            className="rounded-2xl border shadow-sm hover:shadow-md transition overflow-hidden"
          >
            <div className="aspect-[4/3] relative bg-gray-50">
              {p.imageUrl ? (
                <Image src={p.imageUrl} alt={p.name} fill className="object-cover" />
              ) : (
                <div className="absolute inset-0 grid place-items-center text-gray-400">
                  No image
                </div>
              )}

              {p.isOnSale && (
                <span className="absolute left-2 top-2 text-xs bg-red-600 text-white px-2 py-0.5 rounded-full">
                  Flash sale
                </span>
              )}
              {p.preorderStatus === "PREORDER" && (
                <span className="absolute right-2 top-2 text-xs bg-blue-600 text-white px-2 py-0.5 rounded-full">
                  Preorder
                </span>
              )}
            </div>

            <div className="p-4">
              <div className="text-sm text-gray-500">{p.brand || "—"}</div>
              <div className="font-medium">{p.name}</div>
              <div className="mt-1 text-emerald-700 font-semibold">
                {p.price.toFixed(2)} $
              </div>
              <div className="text-xs text-gray-400">{p.slug}</div>
            </div>
          </li>
        ))}
      </ul>
    </main>
  );
}
