import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ILoginRequest } from '../../interfaces/login-request.interface';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.generateForm();
  }

  onSubmit() {
    this.loading = true;
    const loginRequest: ILoginRequest = {
      username: this.loginForm.value.username,
      password: this.loginForm.value.password,
    };

    this.authService.login(loginRequest).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigateByUrl('/home');
        this.toastr.success('Autentificare reusita!');
      },
      error: (err: any) => {
        this.loading = false;
        this.toastr.error('Date de autentificare invalide!', 'Eroare');
      },
    });
  }

  private generateForm() {
    this.loginForm = new FormGroup({
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
    });
  }
}
