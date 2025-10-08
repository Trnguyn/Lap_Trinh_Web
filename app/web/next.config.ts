// app/web/next.config.ts
import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  images: {
    // Cách 1: remotePatterns (khuyến nghị: khai báo chi tiết)
    remotePatterns: [
      // ví dụ bạn đang dùng demo ảnh từ picsum
      { protocol: "https", hostname: "picsum.photos" },

      // ví dụ CDN của bạn
      // { protocol: "https", hostname: "your.cdn.com" },

      // nếu dùng http nội bộ (minio, local service):
      // { protocol: "http", hostname: "localhost", port: "9000" },
      // và có thể giới hạn đường dẫn:
      // { protocol: "https", hostname: "your.cdn.com", pathname: "/images/**" },
    ],

    // Cách 2: domains (ngắn gọn, ít tuỳ biến hơn)
    // domains: ["picsum.photos", "your.cdn.com"],
  },
};

export default nextConfig;
