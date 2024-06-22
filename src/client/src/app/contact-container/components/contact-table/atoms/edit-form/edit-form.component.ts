import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
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
    MatDialogActions,
    MatDialogClose,
    ReactiveFormsModule,
  ],
  template: `
    <div class="edit-form-container">
      <form [formGroup]="createContactForm" (ngSubmit)="onSubmit()">
        <mat-dialog-content class="edit-form-container--input-list">
          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="First Name" formControlName="firstName" />
          </div>

          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="Last Name" formControlName="lastName" />
          </div>

          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="Email" formControlName="email" />
          </div>

          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="Phone" formControlName="phone" />
          </div>

          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="Title" formControlName="title" />
          </div>

          <div class="edit-form-container--input-list--input">
            <input type="text" placeholder="First Name" formControlName="middleInitial" />
          </div>
        </mat-dialog-content>
        <mat-dialog-actions>
          <button [disabled]="createContactForm.invalid" type="submit" cdkFocusInitial>
            Patch
          </button>
        </mat-dialog-actions>
      </form>
    </div>
  `,
  styleUrl: './edit-form.component.scss',
})
export class EditFormComponent {
  private readonly store = inject(Store<AppState>);
  private readonly dialogRef = inject(MatDialogRef<EditFormComponent>);
  private readonly dialogInitData: ContactState = inject(MAT_DIALOG_DATA);

  createContactForm = new FormGroup({
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
          changes: this.createContactForm.value as ContactState
        }
      })
    );

    this.dialogRef.close();
  }
}
