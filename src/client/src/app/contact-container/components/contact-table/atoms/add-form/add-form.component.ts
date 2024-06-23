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
} from '@angular/material/dialog';
import { ContactState } from '../../../../store-features/contact-state.model';
import { AppState } from '../../../../store-features/contact.reducer';
import { Store } from '@ngrx/store';
import { ContactRestActions } from '../../../../store-features/actions/contact-rest.actions';

@Component({
  selector: 'app-add-form',
  standalone: true,
  imports: [
    MatDialogContent,
    ReactiveFormsModule,
  ],
  template: `
    <div class="add-form-container">
      <form [formGroup]="createContactForm" (ngSubmit)="onSubmit()">
        <mat-dialog-content class="add-form-container--input-list">
          <div class="add-form-container--input-list--input">
            <input type="text" placeholder="First Name" formControlName="firstName" />
            @if (createContactForm.controls.firstName.errors && createContactForm.controls.firstName.errors['required']) {
              <small class="add-form-container--input-list--input--error">
                 First Name required
              </small>
            }
          </div>

          <div class="add-form-container--input-list--input">
            <input type="text" placeholder="Last Name" formControlName="lastName" />
            @if (createContactForm.controls.lastName.errors && createContactForm.controls.lastName.errors['required']) {
              <small class="add-form-container--input-list--input--error">
                Last Name required
              </small>
            }
          </div>

          <div class="add-form-container--input-list--input">
            <input type="text" placeholder="Email" formControlName="email" />
            @if (createContactForm.controls.email.errors && createContactForm.controls.email.errors['required']) {
              <small class="add-form-container--input-list--input--error">
                Email required
              </small>
            }
            @if (createContactForm.controls.email.errors && createContactForm.controls.email.errors['email']) {
              <small class="add-form-container--input-list--input--error">
                Wrong email format
              </small>
            }
          </div>

          <div class="add-form-container--input-list--input">
            <input type="text" placeholder="Phone" formControlName="phone" />
            @if (createContactForm.controls.phone.errors && createContactForm.controls.phone.errors['required']) {
              <small class="add-form-container--input-list--input--error">
                Phone required
              </small>
            }
          </div>

          <div class="add-form-container--input-list--input">
            <input type="text" placeholder="Title" formControlName="title" />
            @if (createContactForm.controls.title.errors && createContactForm.controls.title.errors['required']) {
              <small class="add-form-container--input-list--input--error">
                Title required
              </small>
            }
          </div>

          <div class="add-form-container--input-list--input">
            <input type="text" placeholder="Middle Initial" formControlName="middleInitial" />
          </div>

          <div class="add-form-container--input-list--btn">
            <button [disabled]="!createContactForm.valid" type="submit" cdkFocusInitial>
              Create
            </button>
          </div>
        </mat-dialog-content>
      </form>
    </div>
  `,
  styleUrl: './add-form.component.scss',
})
export class AddFormComponent {
  private readonly store = inject(Store<AppState>)
  private readonly dialogRef = inject(MatDialogRef<AddFormComponent>)

  createContactForm = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    phone: new FormControl('', [Validators.required]),
    title: new FormControl('', Validators.required),
    middleInitial: new FormControl(''),
  });

  onSubmit() {
    this.store.dispatch(
      ContactRestActions.addContact({
        contact: this.createContactForm.value as ContactState,
      })
    );

    this.dialogRef.close();
  }
}
