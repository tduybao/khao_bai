using Godot;
using System;
using System.Runtime.CompilerServices;


public partial class Character : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;
    private float dash_speed = 2000;
    private float roll_speed = 1800;
    int clickcount = 0;
    // Tham chiếu đến AnimatedSprite2D
    private AnimatedSprite2D AnimatedSprite2D;

    public override void _Ready()
    {
        // Tìm node B và gán nó vào biến B
        AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        
        // Add gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
            PlayJumpAnimation();
			clickcount = 1;		
        }
        // Double Jump
		if (Input.IsActionJustPressed("ui_accept") && !IsOnFloor() && clickcount == 1)
		{			
			velocity.Y = JumpVelocity;
			PlayDouble_JumpAnimation();
			clickcount = 0;
		}
		

        // Get the input direction and handle the movement/deceleration.
        Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
            PlayRunAnimation();
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            PlayDefaultAnimation();
        }
        //Nhấn Shift để tăng tốc chạy
        if (Input.IsActionPressed("shift")){
            velocity.X *= 2;
        }
        //Thêm dash
        if (Input.IsActionJustPressed("dash")){
            if (AnimatedSprite2D.FlipH){
                velocity.X = -dash_speed; 
            }
            if (!AnimatedSprite2D.FlipH){
                velocity.X = +dash_speed; 
            }
        }
        //Thêm lộn(roll)
        if (IsOnFloor()){
            if (Input.IsActionJustPressed("roll")){
                if (AnimatedSprite2D.FlipH){
                velocity.X -= roll_speed;
                RollAnimation();
                }
                if (!AnimatedSprite2D.FlipH){
                velocity.X += roll_speed; 
                RollAnimation();
                }
            }
        }
        // Chơi animation rơi khi rơi.
        if (!IsOnFloor() && velocity.Y > 0)
        {
            PlayFallingAnimation();
        }

        
        
        Velocity = velocity;
        MoveAndSlide();
        
        //Lật animation
		if  (velocity.X < 0)
		{
			AnimatedSprite2D.FlipH=true;
		}
		else if (velocity.X > 0)
		{
			AnimatedSprite2D.FlipH=false;
		}
	}
    private void PlayRunAnimation()
    {
        if (IsOnFloor() && AnimatedSprite2D.Animation != "running")
        {
            AnimatedSprite2D.Play("running");
        }
    }

    private void PlayJumpAnimation()
    {
        if (AnimatedSprite2D.Animation != "jumping")
        {
            AnimatedSprite2D.Play("jumping");
        }
    }

    private void PlayFallingAnimation()
    {
        if (AnimatedSprite2D.Animation != "falling")
        {
            AnimatedSprite2D.Play("falling");
        }
    }

    private void PlayDefaultAnimation()
    {
        if (IsOnFloor() && Velocity.Length() == 0 && AnimatedSprite2D.Animation != "default")
        {
            AnimatedSprite2D.Play("default");
        }
    }
	private void PlayDouble_JumpAnimation()
    {
        if (!IsOnFloor() && AnimatedSprite2D.Animation == "jumping" || AnimatedSprite2D.Animation == "falling" || AnimatedSprite2D.Animation == "default")
        {
            AnimatedSprite2D.Play("double_jump");
        }
    }
    private void RollAnimation()
    {
        if (IsOnFloor())
        {
            AnimatedSprite2D.Play("roll");
        }

    }
}
