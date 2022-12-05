export interface ResetPasswordRequest {
  email: string;
  password: string;
  confirmPassword: string;
  resetPasswordToken: string;
}
