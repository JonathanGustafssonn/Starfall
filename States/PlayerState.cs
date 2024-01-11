using Starfall.InputManagment;
using Starfall.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.States
{
    /*
      public class PlayerState
      {
          public enum State 
          {
              Idle,
              Jumping,
              Falling,
              Running,
              Dashing
          };
          private State currentState;
          public PlayerState()
          {
              currentState = State.Idle;
          }
          public void Update(Player player, InputManager inputManager)
          {
              switch(currentState)
              {
                  case State.Idle:
                      UpdateIdle(player, inputManager);
                      break;
                  case State.Jumping:
                      UpdateJumping(player, inputManager);
                      break;
                  case State.Falling:
                      UpdateFalling(player, inputManager);
                      break;
                  case State.Running:
                      UpdateRunning(player, inputManager);
                      break;
                  case State.Dashing:
                      UpdateDashing(player, inputManager);
                      break;
              }
          }
          private void UpdateIdle(Player player, InputManager inputManager)
          {
              if(inputManager.IsJumpPressed)
              {
                  currentState = State.Jumping;
              }
              else if(inputManager.IsRunRightPressed || inputManager.IsRunLeftPressed)
              { 
                  currentState = State.Running; 
              }

          }
          private void UpdateJumping(Player player, InputManager inputManager)
          { 
              player.Jump();
          }
          private void UpdateFalling(Player player, InputManager inputManager)
          {
              // add logic later
          }
          private void UpdateRunning(Player player, InputManager inputManager)
          {
              if(!inputManager.IsRunLeftPressed && !inputManager.IsRunRightPressed)
              {
                  currentState = State.Idle;
              }
          }
          private void UpdateDashing(Player player, InputManager inputManager)
          {

          }
      }

  */
}