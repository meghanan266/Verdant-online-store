import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Alert } from '../shared/model/alert-model';
import { User } from '../shared/model/user-model';
import { UserService } from '../shared/service/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public isChangePassword = false;
  public profileForm: FormGroup = new FormGroup({
    userName: new FormControl(null, { validators: Validators.required }),
    email: new FormControl(null, { validators: Validators.required }),
    password: new FormControl(null, { validators: Validators.required }),
    phone: new FormControl(null, { validators: Validators.required }),
  });
  public user: User = new User();
  public alert: Alert;

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    if (this.userService.isLoggedIn()) {
      this.userService.user.subscribe(u => { this.user = u; })
      this.profileForm.reset(this.user);
    }
    else {
      this.router.navigate(["/login"]);
    }
  }

  public onClickSave() {
    this.userService.updateUser(this.profileForm.value).subscribe((res: User) => {
      this.userService.storeToken(res.token);
      this.alert = {
        show: true,
        type: 'success',
        message: 'Saved Successfully!'
      };
      // setTimeout(() => { location.reload() }, 3000);
    });
  }

}
