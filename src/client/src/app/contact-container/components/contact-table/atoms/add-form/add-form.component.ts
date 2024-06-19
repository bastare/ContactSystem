import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
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
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ContactState } from '../../../../store-features/contact-state.model';
import { AppState } from '../../../../store-features/contact.reducer';
import { Store } from '@ngrx/store';
import { ContactRestActions } from '../../../../store-features/actions/contact-rest.actions';

@Component({
  selector: 'app-add-form',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    ReactiveFormsModule,
  ],
  template: `
    <div class="add-form-container">
      <form [formGroup]="createContactForm" (ngSubmit)="onCreate()">
        <mat-dialog-content class="add-form-container--input-list">
          <mat-form-field class="add-form-container--input-list--input">
            <mat-label>First name</mat-label>
            <input matInput placeholder="First Name" formControlName="firstName" />
          </mat-form-field>

          <mat-form-field class="add-form-container--input-list--input">
            <mat-label>Last name</mat-label>
            <input matInput placeholder="Last Name" formControlName="lastName" />
          </mat-form-field>

          <mat-form-field class="add-form-container--input-list--input">
            <mat-label>Email</mat-label>
            <input matInput placeholder="Email" formControlName="email" />
          </mat-form-field>

          <mat-form-field class="add-form-container--input-list--input">
            <mat-label>Phone</mat-label>
            <input matInput placeholder="Phone" formControlName="phone" />
          </mat-form-field>

          <mat-form-field class="add-form-container--input-list--input">
            <mat-label>Title</mat-label>
            <input matInput placeholder="Title" formControlName="title" />
          </mat-form-field>

          <mat-form-field class="add-form-container--input-list--input">
            <mat-label>Middle initial</mat-label>
            <input matInput placeholder="Middle Initial" formControlName="middleInitial" />
          </mat-form-field>
        </mat-dialog-content>
        <mat-dialog-actions>
          <button [disabled]="!createContactForm.valid" type="submit" cdkFocusInitial>
            Create
          </button>
        </mat-dialog-actions>
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

  onCreate() {
    this.createContactForm.validator

    this.store.dispatch(
      ContactRestActions.addContact({
        contact: this.createContactForm.value as ContactState,
      })
    );

    this.dialogRef.close();
  }
}
