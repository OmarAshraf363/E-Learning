import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../core/auth/auth.service';
import { NotifyService } from '../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../shared/forms.util';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { AccountApi } from '../data-access/account.api';

/** Legacy Identity/Account/Manage/Index (phone, description, profile picture). */
@Component({
  selector: 'app-profile-page',
  imports: [ReactiveFormsModule, FormErrorsComponent, FileUrlPipe],
  templateUrl: './profile-page.component.html',
})
export class ProfilePageComponent implements OnInit {
  private readonly api = inject(AccountApi);
  private readonly auth = inject(AuthService);
  private readonly notify = inject(NotifyService);

  readonly userName = signal('');
  readonly picture = signal<string | null>(null);
  readonly saving = signal(false);
  private selectedFile: File | null = null;

  readonly form = inject(FormBuilder).group({
    phoneNumber: [''],
    jopDescription: [''],
    profilePicture: [null as File | null],
  });

  ngOnInit(): void {
    this.api.getProfile().subscribe((profile) => {
      this.userName.set(profile.userName);
      this.picture.set(profile.picture);
      this.form.patchValue({ phoneNumber: profile.phoneNumber ?? '', jopDescription: profile.jopDescription });
    });
  }

  onFileSelected(event: Event): void {
    this.selectedFile = (event.target as HTMLInputElement).files?.[0] ?? null;
  }

  save(): void {
    this.saving.set(true);
    const { phoneNumber, jopDescription } = this.form.getRawValue();

    this.api.updateProfile({ phoneNumber, jopDescription, profilePicture: this.selectedFile }).subscribe({
      next: (profile) => {
        this.saving.set(false);
        this.picture.set(profile.picture);
        this.auth.patchUser({ picture: profile.picture });
        this.notify.success('Your profile has been updated');
      },
      error: (err: unknown) => {
        this.saving.set(false);
        applyServerErrors(this.form, err);
      },
    });
  }
}
