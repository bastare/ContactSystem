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
          <div
            class="add-form-container--input-list--input"
            [class.add-form-container--input-list--input__error]="
              !isValid('firstName', ['required'])
            "
          >
            <input type="text" placeholder="First Name" formControlName="firstName" />

            @if (!isValid('firstName', ['required'])) {
              <small class="add-form-container--input-list--input--error">
                 First Name required
              </small>
            }
          </div>

          <div
            class="add-form-container--input-list--input"
            [class.add-form-container--input-list--input__error]="
              !isValid('lastName', ['required'])
            "
          >
            <input type="text" placeholder="Last Name" formControlName="lastName" />

            @if (!isValid('lastName', ['required'])) {
              <small class="add-form-container--input-list--input--error">
                Last Name required
              </small>
            }
          </div>

          <div
            class="add-form-container--input-list--input"
            [class.add-form-container--input-list--input__error]="
              !isValid('email', ['required', 'email'])
            "
          >
            <input type="text" placeholder="Email" formControlName="email" />

            @if (!isValid('email', ['required'])) {
              <small class="add-form-container--input-list--input--error">
                Email required
              </small>
            }

            @if (!isValid('email', ['email'])) {
              <small class="add-form-container--input-list--input--error">
                Wrong email format
              </small>
            }
          </div>

          <div
            class="add-form-container--input-list--input"
            [class.add-form-container--input-list--input__error]="
              !isValid('phone', ['required'])
            "
          >
            <input type="text" placeholder="Phone" formControlName="phone" />

            @if (!isValid('phone', ['required'])) {
              <small class="add-form-container--input-list--input--error">
                Phone required
              </small>
            }
          </div>

          <div
            class="add-form-container--input-list--input"
            [class.add-form-container--input-list--input__error]="
              !isValid('title', ['required'])
            "
          >
            <input type="text" placeholder="Title" formControlName="title" />

            @if (!isValid('title', ['required'])) {
              <small class="add-form-container--input-list--input--error">
                Title required
              </small>
            }
          </div>

          <div class="add-form-container--input-list--input">
            <input type="text" placeholder="Middle Initial" formControlName="middleInitial" />
          </div>

          <div class="add-form-container--input-list--btn">
            <button [disabled]="!createContactForm.valid" type="submit">
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

  isValid(controlName: string, rules: readonly string[]) {
    return rules.every((rule) => !this.createContactForm.get(controlName)?.getError(rule));
  }
}
