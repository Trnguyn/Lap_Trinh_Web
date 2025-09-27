# Lap_Trinh_Web (Đề tài hiện đang xây dựng)

Clone một phần tính năng của https://jhfigure.com – stack **ASP.NET Core + PostgreSQL + Next.js**.

## Prerequisites
- Docker Desktop
- .NET SDK 8
- Node.js 20 + pnpm

## Quick start
```bash
# 1) Database
cd docker
docker compose up -d

# 2) API
cd ../app/api/JHF.Api
dotnet run
# Now listening on: http://localhost:5071

# 3) Web
cd ../../web
echo NEXT_PUBLIC_API_BASE=http://localhost:5071 > .env.local
pnpm i
pnpm dev   # http://localhost:3000


## Cách cập nhật dần
- Mỗi lần thêm endpoint (vd `/api/brands`, filter `search/brandId/sort`), thêm 1–2 dòng ở mục **API**.  
- Khi chốt trang chi tiết sản phẩm, chèn ảnh minh hoạ vào README.  
- Trước khi nộp, thêm mục **Kiến trúc** (mô tả ngắn 2–3 box) và **Roadmap** (todo đã/đang/làm sau).

## Lệnh tạo README ngay (PowerShell)
```powershell
@"
# Lap_Trinh_Web
... (dán nội dung mẫu ở trên) ...
"@ | Set-Content -Encoding UTF8 README.md

git add README.md
git commit -m "docs: add minimal README with quick start"
git push
