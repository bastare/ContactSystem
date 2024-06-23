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
          </div>

          <div class="add-form-container--input-list--input">
            <input type="text" placeholder="Last Name" formControlName="lastName" />
          </div>

          <div class="add-form-container--input-list--input">
            <input type="text" placeholder="Email" formControlName="email" />
          </div>

          <div class="add-form-container--input-list--input">
            <input type="text" placeholder="Phone" formControlName="phone" />
          </div>

          <div class="add-form-container--input-list--input">
            <input type="text" placeholder="Title" formControlName="title" />
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
