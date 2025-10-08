// Fallback: nếu API không trả imageUrl thì dùng ảnh local theo slug
export const productImage = (slug: string, remote?: string | null) =>
  remote && remote.length > 0 ? remote : `/images/products/${slug}.jpg`;
