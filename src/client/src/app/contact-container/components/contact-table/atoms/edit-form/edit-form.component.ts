import { Component, Inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
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
import { Contact } from '../../../../store-features/contact.model';
import { State } from '../../../../store-features/contact.reducer';
import { Store } from '@ngrx/store';
import { ContactActions } from '../../../../store-features/actions/contact.actions';

@Component({
  selector: 'app-edit-form',
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
    <form [formGroup]="createContactForm" (ngSubmit)="onCreate()">
      <mat-dialog-content>
        <mat-form-field>
          <mat-label>First name</mat-label>
          <input matInput placeholder="First Name" formControlName="firstName" />
        </mat-form-field>

        <mat-form-field>
          <mat-label>Last name</mat-label>
          <input matInput placeholder="Last Name" formControlName="lastName" />
        </mat-form-field>

        <mat-form-field>
          <mat-label>Email</mat-label>
          <input matInput placeholder="Email" formControlName="email" />
        </mat-form-field>

        <mat-form-field>
          <mat-label>Phone</mat-label>
          <input matInput placeholder="Phone" formControlName="phone" />
        </mat-form-field>

        <mat-form-field>
          <mat-label>Title</mat-label>
          <input matInput placeholder="Title" formControlName="title" />
        </mat-form-field>

        <mat-form-field>
          <mat-label>Middle initial</mat-label>
          <input matInput placeholder="First Name" formControlName="middleInitial" />
        </mat-form-field>
      </mat-dialog-content>
      <mat-dialog-actions>
        <button mat-button type="submit" cdkFocusInitial>Ok</button>
      </mat-dialog-actions>
    </form>
  `,
  styleUrl: './edit-form.component.scss',
})
export class EditFormComponent {
  createContactForm = new FormGroup({
    firstName: new FormControl(this.data.firstName),
    lastName: new FormControl(this.data.lastName),
    email: new FormControl(this.data.email),
    phone: new FormControl(this.data.phone),
    title: new FormControl(this.data.title),
    middleInitial: new FormControl(this.data.middleInitial),
  });

  onCreate() {
    this.store.dispatch(
      ContactActions['[REST/API]UpdateContact']({
        contact: {
          ...this.createContactForm.value as Contact,
          id: this.data.id
        },
      })
    );

    this.dialogRef.close();
  }

  constructor(
    private readonly store: Store<State>,
    private readonly dialogRef: MatDialogRef<EditFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Contact
  ) { }
}
