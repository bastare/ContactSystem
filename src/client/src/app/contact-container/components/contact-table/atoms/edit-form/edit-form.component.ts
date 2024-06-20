import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ContactState } from '../../../../store-features/contact-state.model';
import { AppState } from '../../../../store-features/contact.reducer';
import { Store } from '@ngrx/store';
import { ContactRestActions } from '../../../../store-features/actions/contact-rest.actions';

@Component({
  selector: 'app-edit-form',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    ReactiveFormsModule,
  ],
  template: `
    <div class="edit-form-container">
      <form [formGroup]="createContactForm" (ngSubmit)="onEdit()">
        <mat-dialog-content class="edit-form-container--input-list">
          <mat-form-field class="edit-form-container--input-list--input">
            <mat-label>First name</mat-label>
            <input matInput placeholder="First Name" formControlName="firstName" />
          </mat-form-field>

          <mat-form-field class="edit-form-container--input-list--input">
            <mat-label>Last name</mat-label>
            <input matInput placeholder="Last Name" formControlName="lastName" />
          </mat-form-field>

          <mat-form-field class="edit-form-container--input-list--input">
            <mat-label>Email</mat-label>
            <input matInput placeholder="Email" formControlName="email" />
          </mat-form-field>

          <mat-form-field class="edit-form-container--input-list--input">
            <mat-label>Phone</mat-label>
            <input matInput placeholder="Phone" formControlName="phone" />
          </mat-form-field>

          <mat-form-field class="edit-form-container--input-list--input">
            <mat-label>Title</mat-label>
            <input matInput placeholder="Title" formControlName="title" />
          </mat-form-field>

          <mat-form-field class="edit-form-container--input-list--input">
            <mat-label>Middle initial</mat-label>
            <input matInput placeholder="First Name" formControlName="middleInitial" />
          </mat-form-field>
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

  onEdit() {
    this.store.dispatch(
      ContactRestActions.updateContact({
        contact: {
          id: this.dialogInitData.id,
          changes: {
            ...this.createContactForm.value as ContactState,
          }
        },
      })
    );

    this.dialogRef.close();
  }
}
