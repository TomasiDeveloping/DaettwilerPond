import {Component, inject, OnInit} from '@angular/core';
import {UserWithAddress} from "../../../models/user.model";
import {UserService} from "../../../services/user.service";
import {ToastrService} from "ngx-toastr";
import {MatDialog} from "@angular/material/dialog";
import {AdminEditUserComponent} from "./admin-edit-user/admin-edit-user.component";
import {GridModel} from "@syncfusion/ej2-angular-grids";
import Swal from "sweetalert2";


@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.scss']
})
export class AdminUsersComponent implements OnInit {
  public users: UserWithAddress[] = [];
  public addressChildGrid!: GridModel;

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
        {field: 'address.street', headerText: 'Strasse'},
        {field: 'address.houseNumber', headerText: 'Hausnummer'},
        {field: 'address.postalCode', headerText: 'PLZ'},
        {field: 'address.city', headerText: 'Ort'},
        {field: 'address.phone', headerText: 'Telefon'},
        {field: 'address.mobile', headerText: 'Natel'}
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
    dialogRef.afterClosed().subscribe((result: {reload: boolean}) => {
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
}
