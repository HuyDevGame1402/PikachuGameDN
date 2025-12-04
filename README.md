# PikachuGameDN
Đồ án Game Pikachu

1. Xây dựng lever Game
   Mỗi 1 lever sẽ có các hàng và cột khác nhau
   Có ds các ô có thể bỏ trống
  
2. Xây dựng Logic gameplay
   Bắt va chạm bằng collision2D
   Tìm các ô trùng id có thể ăn được (thỏa mãn điều kiện game)
   Tạo ra bảng đạt yêu cầu phải có ít nhất 1 cặp có thể ăn được k thì sẽ tạo lại
   Tạo ra các VFX khi ăn được cho game hay hơn

3. Tối ưu game
   Sử dụng giải thuật để giảm bớt duyệt ma trận, tính toán, loại bỏ các cặp trùng nhau khi xét duyệt khởi tạo bảng cell
   Sử dụng ObjectPool tránh spawn ra nhiều GameObject giúp tái sử dụng
