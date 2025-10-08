// Định dạng tiền tệ USD theo locale VN (hiển thị $ ở đầu)
export const money = (v: number) =>
  new Intl.NumberFormat("vi-VN", { style: "currency", currency: "USD" })
    .format(v)
    .replace("US$", "$");
