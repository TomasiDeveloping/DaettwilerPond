import {Component, inject, OnInit} from '@angular/core';
import {UserWithAddress} from "../../../models/user.model";
import {UserService} from "../../../services/user.service";
import {ToastrService} from "ngx-toastr";
import {MatDialog} from "@angular/material/dialog";
import {AdminEditUserComponent} from "./admin-edit-user/admin-edit-user.component";
import {GridModel, ToolbarItems} from "@syncfusion/ej2-angular-grids";
import Swal from "sweetalert2";
import {AdminSendEmailComponent} from "./admin-send-email/admin-send-email.component";


@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html'
})
export class AdminUsersComponent implements OnInit {
  public users: UserWithAddress[] = [];
  public addressChildGrid!: GridModel;
  public pageSettings = {pageSizes: true, pageSize: 10};
  public toolbarOptions: ToolbarItems[] = ['Search'];

  private readonly _userService: UserService = inject(UserService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _dialog: MatDialog = inject(MatDialog);

  ngOnInit() {
    this.getUsers();
  }

  getUsers() {
    this._userService.getUsersWithAddresses().subscribe({
      next: ((response) => {
        if (response) {
          this.users = response;
          this.configChildGrid(response);
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Error getUsers', 'Users');
      }
    });
  }

  onEdit(event: any) {
    const user: UserWithAddress = event.rowData as UserWithAddress;
    this.openEditUserDialog(true, user);
  }

  configChildGrid(users: UserWithAddress[]) {
    this.addressChildGrid = {
      dataSource: users,
      queryString: 'userId',
      columns: [
        {field: 'address.street', headerText: 'Strasse', textAlign: 'Center'},
        {field: 'address.houseNumber', headerText: 'Hausnummer', textAlign: 'Center'},
        {field: 'address.postalCode', headerText: 'PLZ', textAlign: 'Center'},
        {field: 'address.city', headerText: 'Ort', textAlign: 'Center'},
        {field: 'address.phone', headerText: 'Telefon', textAlign: 'Center'},
        {field: 'address.mobile', headerText: 'Natel', textAlign: 'Center'}
      ]
    };
  }

  onAddUser() {
    const user: UserWithAddress = {
      email: '',
      lastName: '',
      firstName: '',
      isActive: true,
      userId: '',
      role: 'User',
      address: {
        street: '',
        userId: '',
        mobile: '',
        phone: '',
        country: '',
        city: '',
        postalCode: '',
        houseNumber: '',
        id: ''
      }
    };
    this.openEditUserDialog(false, user);
  }

  openEditUserDialog(isUpdate: boolean, user: UserWithAddress) {
    const dialogRef = this._dialog.open(AdminEditUserComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      disableClose: true,
      data: {isUpdate: isUpdate, user: user}
    });
    dialogRef.afterClosed().subscribe((result: { reload: boolean }) => {
      if (result.reload) {
        this.getUsers();
      }
    });
  }

  onDeleteUser(user: UserWithAddress) {
    Swal.fire({
      title: 'Bist Du sicher?',
      text: `${user.firstName} ${user.lastName} wirklisch löschen?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Ja, bitte löschen!',
      cancelButtonText: 'Abbrechen'
    }).then((result) => {
      if (result.isConfirmed) {
        this._userService.deleteUser(user.userId).subscribe({
          next: ((response) => {
            if (response) {
              this._toastr.success(`User ${user.firstName} wurde erfolgreich gelöscht`, 'User Löschen');
              this.getUsers();
            }
          }),
          error: error => {
            this._toastr.error(error.error ?? 'User konnte nicht gelöscht werden', 'User Löschen');
          }
        });
      }
    });
  }

  onSendEmails() {
    const memberEmails: {email: string, fullName: string}[] = [];
    this.users.forEach((user) => {
      memberEmails.push({email: user.email, fullName: `${user.firstName} ${user.lastName}`})
    });
    this._dialog.open(AdminSendEmailComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      disableClose: true,
      data: {memberEmails: memberEmails}
    })
  }
}
