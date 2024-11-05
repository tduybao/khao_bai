using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Item : Area2D
{
    // Kiểm tra va chạm
    public override void _Ready()
    {
        // Kết nối tín hiệu body_entered với phương thức OnBodyEntered
        this.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        // Kiểm tra xem vật thể va chạm có phải là player không
        if (body is Character)
        {
            // Thực hiện hành động, ví dụ như thêm điểm, thay đổi sprite, v.v.
            GD.Print("Player đã chạm vào vật phẩm!");

            // Ẩn hoặc xóa vật phẩm sau khi nhặt
            QueueFree();
        }
    }
}
