// import { Component } from '@angular/core';
// import { MatDialog } from '@angular/material/dialog';
// import { Store } from '@ngrx/store';
// import { AppState } from '../../../../store-features/contact.reducer';

// @Component({
//   selector: 'app-table',
//   standalone: true,
//   imports: [],
//   template: `
//     <table mat-table [dataSource]="dataSource" matSort>
//           <ng-container matColumnDef="id">
//             <th mat-header-cell *matHeaderCellDef mat-sort-header>ID</th>
//             <td mat-cell *matCellDef="let contact">{{ contact.id }}</td>
//           </ng-container>

//           <ng-container matColumnDef="firstName">
//             <th mat-header-cell *matHeaderCellDef mat-sort-header>
//               First Name
//             </th>
//             <td mat-cell *matCellDef="let contact">{{ contact.firstName }}</td>
//           </ng-container>

//           <ng-container matColumnDef="lastName">
//             <th mat-header-cell *matHeaderCellDef mat-sort-header>Last Name</th>
//             <td mat-cell *matCellDef="let contact">{{ contact.lastName }}</td>
//           </ng-container>

//           <ng-container matColumnDef="email">
//             <th mat-header-cell *matHeaderCellDef mat-sort-header>Email</th>
//             <td mat-cell *matCellDef="let contact">{{ contact.email }}</td>
//           </ng-container>

//           <ng-container matColumnDef="phone">
//             <th mat-header-cell *matHeaderCellDef mat-sort-header>Phone</th>
//             <td mat-cell *matCellDef="let contact">{{ contact.phone }}</td>
//           </ng-container>

//           <ng-container matColumnDef="title">
//             <th mat-header-cell *matHeaderCellDef mat-sort-header>Title</th>
//             <td mat-cell *matCellDef="let contact">{{ contact.title }}</td>
//           </ng-container>

//           <ng-container matColumnDef="middleInitial">
//             <th mat-header-cell *matHeaderCellDef mat-sort-header>
//               Middle Initial
//             </th>
//             <td mat-cell *matCellDef="let contact">
//               {{ contact.middleInitial }}
//             </td>
//           </ng-container>

//           <ng-container matColumnDef="actions">
//             <th mat-header-cell *matHeaderCellDef mat-sort-header>Actions</th>
//             <td mat-cell *matCellDef="let contact">
//               <button mat-button (click)="openEditContactDialog(contact)">
//                 Edit
//               </button>
//               <button mat-button (click)="removeContact(contact.id)">
//                 Delete
//               </button>
//             </td>
//           </ng-container>

//           <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
//           <tr mat-row *matRowDef="let contact; columns: displayedColumns"></tr>

//           <tr class="mat-row" *matNoDataRow>
//             <td class="mat-cell" colspan="4">No data matching the filter</td>
//           </tr>
//         </table>
//   `,
//   styleUrl: './table.component.scss'
// })
// export class TableComponent {


//   constructor(
//     private readonly store: Store<AppState>,
//     private readonly dialog: MatDialog
//   ) {}
// }
