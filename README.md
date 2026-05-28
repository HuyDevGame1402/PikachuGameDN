# 🐾 PikachuGame — Classic Pikachu & Sliding Puzzle

![Unity](https://img.shields.io/badge/Unity-6-black?logo=unity)
![PlayFab](https://img.shields.io/badge/Microsoft-PlayFab-green)
![Platform](https://img.shields.io/badge/Platform-PC)
![Mode](https://img.shields.io/badge/Mode-Offline%20%2F%20Online-blue)

Một tựa game giải đố kết hợp giữa **Pikachu cổ điển** và cơ chế **trượt ô theo hàng/cột kiểu Candy Crush Saga**, được phát triển trên nền tảng **Unity**. Người chơi vừa ghép đôi các ô hình ảnh theo luật Pikachu truyền thống, vừa thao tác trượt ngang/dọc để tạo ra các cặp ăn khớp — mang đến trải nghiệm mới mẻ, kích thích tư duy và giải trí cao. Tiến trình chơi được lưu lên cloud qua **Microsoft PlayFab**.

---

## 🛠️ Công Nghệ Sử Dụng (Tech Stack)

| Công cụ | Phiên bản | Vai trò |
|---|---|---|
| **Unity** | 6 (6000.x LTS) | Engine phát triển game chính |
| **Microsoft PlayFab** | Latest SDK | Lưu tiến trình, level hoàn thành, điểm cao |
| **C#** | .NET 9 | Ngôn ngữ lập trình chính |
| **TextMesh Pro** | Built-in | Hiển thị văn bản UI sắc nét mọi độ phân giải |

---

## 📌 Tính Năng Nổi Bật (Features)

### 🎮 Chế Độ Pikachu Cổ Điển
- Ghép đôi 2 ô có cùng hình ảnh theo đường đi không quá 3 khúc ngoặt
- Hỗ trợ gợi ý (Hint) khi người chơi bị kẹt
- Xáo trộn bàn cờ (Shuffle) khi không còn nước đi hợp lệ
- Đếm ngược thời gian, tính điểm theo tốc độ ghép đôi

### 🍬 Chế Độ Trượt Ô (Sliding Puzzle)
- **Trượt ngang:** Kéo cả hàng sang trái / phải để căn chỉnh vị trí ô
- **Trượt dọc:** Kéo cả cột lên / xuống tạo tổ hợp mới
- Kết hợp cơ chế trượt + ghép đôi Pikachu trong cùng một lượt chơi
- Độ khó tăng dần qua từng level (kích thước bảng, số loại hình ảnh)

### 💾 Lưu Tiến Trình (PlayFab Cloud Save)
- Tự động lưu level hiện tại, điểm cao (High Score) lên **PlayFab Player Data**
- Đồng bộ tiến trình trên nhiều thiết bị qua tài khoản PlayFab
- Lưu trạng thái màn chơi (số sao đạt được, thời gian hoàn thành tốt nhất)

### 🗺️ Hệ Thống Map & Level
- Màn hình chọn map với các level được mở khóa dần
- Mỗi map có chủ đề hình ảnh riêng (Pokemon, thiên nhiên, đồ vật...)
- `ScriptableObject` quản lý cấu hình từng level: kích thước lưới, loại ô, giới hạn thời gian

### ✨ Hiệu Ứng & Âm Thanh
- VFX bùng nổ khi ghép đôi thành công (`Vfx/`)
- Hiệu ứng trượt mượt mà với animation tween
- Âm thanh riêng biệt cho từng hành động: ghép đúng, ghép sai, hết giờ, qua màn

---

## 📁 Cấu Trúc Thư Mục (Assets Structure)

```text
Assets/
├── Animation/                   # Animation Clip: hiệu ứng trượt ô, ghép đôi, UI transition
│
├── 🎨 UI & Visual
│   ├── 2D Casual UI/            # Bộ UI casual: nút, panel, popup, thanh tiến trình
│   ├── Space_Exploration_GUI_Kit/ # UI Kit cho màn hình chính, loading, bảng điểm
│   └── Sprites/                 # Hình ảnh 2D: icon ô cờ, nền map, nhân vật, avatar
│
├── 🎮 Gameplay & Data
│   ├── Prefabs/                 # Prefab: ô cờ, bảng lưới, hiệu ứng ghép đôi, UI elements
│   ├── SccriptTableObjectLever/ # ScriptableObject cấu hình từng level (grid size, tile type, timer)
│   └── Resources/               # Tài nguyên load động tại runtime (tile sprites, level config)
│
├── ✨ Effects
│   └── Vfx/                     # Visual Effects: nổ ô, tia sáng ghép đôi, hiệu ứng combo
│
├── 🌐 Backend
│   ├── PlayFabSDK/              # Microsoft PlayFab SDK: Player Data, Statistics, CloudScript
│   └── PlayFabEditorExtensions/ # Extension cấu hình PlayFab Title ID trong Unity Editor
│
├── 🔧 Core & Config
│   ├── Plugins/                 # Thư viện DLL bên thứ ba
│   ├── Scripts/                 # Toàn bộ mã nguồn C# (xem chi tiết bên dưới)
│   ├── Scenes/                  # Các Scene: Boot, MainMenu, MapSelect, Gameplay, Result
│   ├── Settings/                # URP Settings, Input System, Audio Mixer, Quality Settings
│   └── TextMesh Pro/            # Cấu hình TextMesh Pro — hiển thị chữ UI sắc nét
│
└── 🎵 Audio
    └── Sounds/                  # Nhạc nền BGM + SFX: ghép đôi, trượt ô, qua màn, thất bại
```

---

## 🛠️ Kiến Trúc Mã Nguồn (Scripts Architecture)

### 💾 Backend & Save System
| Script | Chức năng |
|---|---|
| `PlayFabAuthManager.cs` | Đăng nhập ẩn danh / tài khoản, khởi tạo phiên PlayFab |
| `PlayFabSaveManager.cs` | Lưu / tải tiến trình: level hiện tại, điểm cao, số sao |
| `PlayFabStatistics.cs` | Cập nhật bảng xếp hạng điểm cao (Leaderboard) |

### 🗺️ Level & Map System
| Script | Chức năng |
|---|---|
| `LevelManager.cs` | Quản lý danh sách level, trạng thái mở khóa, chuyển màn |
| `LevelConfig.cs` | ScriptableObject chứa cấu hình level: kích thước lưới, loại tile, timer |
| `MapSelectUI.cs` | Hiển thị màn hình chọn map, render trạng thái sao từng level |

### 🎮 Pikachu Core Logic
| Script | Chức năng |
|---|---|
| `BoardManager.cs` | Khởi tạo lưới, phân phối ngẫu nhiên các cặp ô, quản lý trạng thái bảng |
| `TileController.cs` | Xử lý click chọn ô, highlight, kiểm tra cặp hợp lệ |
| `PathFinder.cs` | Thuật toán tìm đường nối 2 ô (tối đa 3 khúc ngoặt) |
| `HintSystem.cs` | Tìm và gợi ý cặp ô có thể ghép hợp lệ |
| `ShuffleManager.cs` | Xáo trộn lại bảng khi không còn nước đi hợp lệ |

### 🍬 Sliding Puzzle Logic
| Script | Chức năng |
|---|---|
| `SlideController.cs` | Xử lý input kéo hàng/cột, tính toán vị trí trượt |
| `HorizontalSlider.cs` | Trượt toàn bộ hàng sang trái / phải với animation |
| `VerticalSlider.cs` | Trượt toàn bộ cột lên / xuống với animation |
| `SlideValidator.cs` | Kiểm tra sau mỗi lần trượt có tạo ra cặp hợp lệ không |

### ⏱️ Game Flow & UI
| Script | Chức năng |
|---|---|
| `GameManager.cs` | Quản lý trạng thái game: Start, Pause, Win, Lose, TimeOut |
| `TimerController.cs` | Đếm ngược thời gian, cảnh báo khi sắp hết giờ |
| `ScoreManager.cs` | Tính điểm theo tốc độ, combo ghép liên tiếp, cộng thưởng |
| `ResultScreen.cs` | Màn hình kết quả: điểm, số sao, nút chơi lại / màn tiếp theo |

---

## 🚀 Hướng Dẫn Cài Đặt (Installation & Setup)

### 🖥️ Yêu Cầu Hệ Thống (Prerequisites)
- **Unity Editor:** `6000.x LTS`
- **PlayFab SDK:** Đã tích hợp sẵn trong thư mục `PlayFabSDK/`
- **IDE:** Visual Studio 2022 / JetBrains Rider / VS Code

### 📋 Các Bước Thực Hiện

**1. Clone mã nguồn từ GitHub**
```bash
git clone https://github.com/[ten-tai-khoan]/PikachuGame.git
```

**2. Mở dự án bằng Unity Hub**
- Mở **Unity Hub** → **Add → Add project from disk**
- Chọn phiên bản `Unity 6000.x LTS` và mở dự án
- Nếu có thông báo TextMesh Pro → chọn **Import TMP Essentials**

**3. Cấu hình Microsoft PlayFab**
- Truy cập [PlayFab Developer Portal](https://developer.playfab.com)
- Tạo **Title** mới, sao chép `Title ID`
- Trong Unity: **PlayFab → Editor Extensions** → dán `Title ID` và đăng nhập

**4. Cấu hình Build Settings**
- Vào **File → Build Settings**, thêm Scene theo thứ tự:
```
Assets/Scenes/LoginScene.unity
Assets/Scenes/GameLevelMap.unity
Assets/Scenes/GameScene.unity
```

**5. Chạy và kiểm thử**
- Mở Scene `MainMenu` → nhấn **▶ Play** để bắt đầu
- Kiểm tra lưu tiến trình: hoàn thành 1 level → thoát → vào lại xem level có được lưu không

---

## 📝 Bản Quyền & Ghi Chú (License & Notes)

- **Trạng thái:** Đang phát triển — Core Gameplay hoàn thiện
- **Asset bên thứ ba** (2D Casual UI, Space Exploration GUI Kit, v.v.) thuộc bản quyền tác giả trên Unity Asset Store — vui lòng không thương mại hóa khi chưa được cấp phép
- **Mã nguồn Scripts** được phát triển nội bộ bởi đội ngũ dự án

> 💡 Dự án được thiết kế **modular** — dễ dàng thêm map mới, chủ đề hình ảnh mới hoặc cơ chế trượt mới mà không ảnh hưởng đến core logic.

---

## 👤 Tác Giả (Author)

| | |
|---|---|
| **Họ và Tên** | Nguyễn Đức Huy |
| **Email** | [huyco14022004@gmail.com](mailto:huyco14022004@gmail.com) |
| **LinkedIn** | [nguyễn-đức-huy](https://www.linkedin.com/in/nguy%E1%BB%85n-%C4%91%E1%BB%A9c-huy-081a73411/) |
