import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  MatDialogContent,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { ContactState } from '../../../../store-features/contact-state.model';
import { AppState } from '../../../../store-features/contact.reducer';
import { Store } from '@ngrx/store';
import { ContactRestActions } from '../../../../store-features/actions/contact-rest.actions';

@Component({
  selector: 'app-edit-form',
  standalone: true,
  imports: [
    MatDialogContent,
    ReactiveFormsModule,
  ],
  template: `
    <div class="edit-form-container">
      <form [formGroup]="patchContactForm" (ngSubmit)="onSubmit()">
        <mat-dialog-content class="edit-form-container--input-list">
          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="First Name" formControlName="firstName" />
            @if (patchContactForm.controls.firstName.errors && patchContactForm.controls.firstName.errors['required']) {
              <small class="edit-form-container--input-list--input--error">
                 First Name required
              </small>
            }
          </div>

          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="Last Name" formControlName="lastName" />
            @if (patchContactForm.controls.lastName.errors && patchContactForm.controls.lastName.errors['required']) {
              <small class="edit-form-container--input-list--input--error">
                 Last Name required
              </small>
            }
          </div>

          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="Email" formControlName="email" />
            @if (patchContactForm.controls.email.errors && patchContactForm.controls.email.errors['required']) {
              <small class="edit-form-container--input-list--input--error">
                 Email required
              </small>
            }
            @if (patchContactForm.controls.email.errors && patchContactForm.controls.email.errors['email']) {
              <small class="edit-form-container--input-list--input--error">
                 Wrong email format
              </small>
            }
          </div>

          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="Phone" formControlName="phone" />
            @if (patchContactForm.controls.phone.errors && patchContactForm.controls.phone.errors['required']) {
              <small class="edit-form-container--input-list--input--error">
                Phone required
              </small>
            }
          </div>

          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="Title" formControlName="title" />
            @if (patchContactForm.controls.title.errors && patchContactForm.controls.title.errors['required']) {
              <small class="edit-form-container--input-list--input--error">
                Title required
              </small>
            }
          </div>

          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="First Name" formControlName="middleInitial" />
          </div>
          <div class="edit-form-container--input-list--btn">
            <button [disabled]="patchContactForm.invalid" type="submit" cdkFocusInitial>
              Patch
            </button>
          </div>
        </mat-dialog-content>
      </form>
    </div>
  `,
  styleUrl: './edit-form.component.scss',
})
export class EditFormComponent {
  private readonly store = inject(Store<AppState>);
  private readonly dialogRef = inject(MatDialogRef<EditFormComponent>);
  private readonly dialogInitData: ContactState = inject(MAT_DIALOG_DATA);

  patchContactForm = new FormGroup({
    firstName: new FormControl(this.dialogInitData.firstName, Validators.required),
    lastName: new FormControl(this.dialogInitData.lastName, Validators.required),
    email: new FormControl(this.dialogInitData.email, [Validators.required, Validators.email]),
    phone: new FormControl(this.dialogInitData.phone, Validators.required),
    title: new FormControl(this.dialogInitData.title, Validators.required),
    middleInitial: new FormControl(this.dialogInitData.middleInitial),
  });

  onSubmit() {
    this.store.dispatch(
      ContactRestActions.updateContact({
        contact: {
          id: this.dialogInitData.id,
          changes: this.patchContactForm.value as ContactState
        }
      })
    );

    this.dialogRef.close();
  }
}
