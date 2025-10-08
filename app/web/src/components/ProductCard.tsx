import Image from "next/image";
import { Card, CardBody } from "./ui/Card";
import Badge from "./ui/Badge";
import Button from "./ui/Button";
import { money } from "@/lib/currency";
import { productImage } from "@/lib/img";

export type ProductItem = {
  id: number;
  name: string;
  slug: string;
  brand: string;
  imageUrl: string | null;
  price: number;
  isOnSale: boolean;
  preorderStatus: string;
};

export default function ProductCard({ p }: { p: ProductItem }) {
  const cover = productImage(p.slug, p.imageUrl);
  return (
    <Card className="flex flex-col">
      <div className="relative aspect-[4/3]">
        <Image
          src={cover}
          alt={p.name}
          fill
          sizes="(min-width:1024px) 25vw, (min-width:768px) 33vw, 100vw"
          className="object-cover"
        />
        <div className="absolute left-3 top-3 flex gap-2">
          {p.preorderStatus === "PREORDER" && <Badge color="blue">Preorder</Badge>}
          {p.isOnSale && <Badge color="red">Flash sale</Badge>}
        </div>
      </div>
      <CardBody className="flex flex-col gap-1">
        <div className="text-sm text-white/60">{p.brand}</div>
        <div className="font-semibold">{p.name}</div>
        <div className="mt-2 flex items-baseline gap-2">
          <div className="text-emerald-400 font-semibold">{money(p.price)}</div>
        </div>
        <div className="mt-4 grid grid-cols-2 gap-2">
          <Button variant="outline" onClick={() => (window.location.href = `/product/${p.slug}`)}>
            Chi tiết
          </Button>
          <Button>Thêm vào giỏ</Button>
        </div>
      </CardBody>
    </Card>
  );
}
