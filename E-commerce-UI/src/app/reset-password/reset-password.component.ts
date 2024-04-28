import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ResetPassword } from '../shared/model/reset-password-model';
import { ResetPasswordService } from '../shared/service/reset-password.service';
import { Alert } from '../shared/model/alert-model';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

  public emailToken: string;
  public emailToReset: string;
  public alert: Alert;
  public resetPwdObj: ResetPassword = new ResetPassword;
  public resetForm = this.fb.group({
    newPassword: new FormControl(null, { validators: Validators.required }),
    confirmPassword: new FormControl(null, { validators: Validators.required })
  }, {
    validator: this.confirmPasswordValidator("newPassword", "confirmPassword")
  });

  constructor(private fb: FormBuilder, private activatedRoute: ActivatedRoute, private resetPwdService: ResetPasswordService,
    private router: Router) {
    this.activatedRoute.queryParams.subscribe(val => {
      this.emailToReset = val['email'];
      this.emailToken = val['code'].replace(/ /g, '+')
    })
  }

  ngOnInit(): void {
  }

  onClickConfirm() {
    this.resetPwdObj.email = this.emailToReset;
    this.resetPwdObj.emailToken = this.emailToken;
    this.resetPwdObj.newPassword = this.resetForm.value.newPassword;
    this.resetPwdObj.confirmPassword = this.resetForm.value.confirmPassword;

    this.resetPwdService.resetPassword(this.resetPwdObj).subscribe({
      next: (res) => {
        this.alert = {
          show: true,
          type: 'success',
          message: 'Password reset successful!'
        };
        this.router.navigate(["/login"]);
      },
      error: (err) => {
        this.alert = {
          show: true,
          type: 'fail',
          message: 'Something went wrong.'
        };
      }
    })
  }

  confirmPasswordValidator(controlName: string, matchControlName: string) {
    return (formGroup: FormGroup) => {
      const pwdControl = formGroup.controls[controlName];
      const confirmPwdControl = formGroup.controls[matchControlName];
      if (pwdControl.value !== confirmPwdControl.value) {
        confirmPwdControl.setErrors({ confirmPasswordValidator: true });
      } else {
        confirmPwdControl.setErrors(null);
      }
    }
  }

}
