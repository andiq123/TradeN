import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { PasswordValidators } from 'src/app/core/_validators/PasswordValidators';
import { IRegisterRequest } from '../../interfaces/register-request.interface';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup = new FormGroup({});
  loading = false;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.generateForm();
  }

  onSubmit() {
    this.loading = true;

    const registerRequest: IRegisterRequest = {
      username: this.registerForm.value.username,
      email: this.registerForm.value.email,
      fullName: this.registerForm.value.fullName,
      location: this.registerForm.value.location,
      phoneNumber: this.registerForm.value.phoneNumber,
      password: this.registerForm.value.password,
    };

    this.authService.register(registerRequest).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigateByUrl('/home');
      },
      error: (err: any) => {
        this.loading = false;
        console.log(err);
      },
    });
  }

  private generateForm() {
    this.registerForm = new FormGroup(
      {
        username: new FormControl(
          { value: '', disabled: false },
          {
            validators: [
              Validators.required,
              Validators.minLength(5),
              Validators.maxLength(10),
            ],
          }
        ),
        email: new FormControl(
          { value: '', disabled: false },
          {
            validators: [
              Validators.required,
              Validators.email,
              Validators.minLength(5),
              Validators.maxLength(30),
            ],
          }
        ),
        fullName: new FormControl(
          { value: '', disabled: false },
          {
            validators: [
              Validators.required,
              Validators.minLength(5),
              Validators.maxLength(30),
            ],
          }
        ),
        location: new FormControl(
          { value: '', disabled: false },
          {
            validators: [
              Validators.required,
              Validators.minLength(5),
              Validators.maxLength(10),
            ],
          }
        ),
        phoneNumber: new FormControl(
          { value: '', disabled: false },
          {
            validators: [
              Validators.required,
              Validators.minLength(5),
              Validators.maxLength(10),
            ],
          }
        ),
        password: new FormControl(
          { value: '', disabled: false },
          {
            validators: [
              Validators.required,
              Validators.minLength(5),
              Validators.maxLength(10),
            ],
          }
        ),
        confirmPassword: new FormControl(
          { value: '', disabled: false },
          {
            validators: [
              Validators.required,
              Validators.minLength(5),
              Validators.maxLength(10),
            ],
          }
        ),
      },
      { validators: [PasswordValidators.MatchPassword] }
    );
  }
}
