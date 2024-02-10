import {Component, inject, OnInit} from '@angular/core';
import {UserService} from "../../services/user.service";
import {User} from "../../models/user.model";
import {MatDialog} from "@angular/material/dialog";
import {AdminSendEmailComponent} from "../admin/admin-users/admin-send-email/admin-send-email.component";

@Component({
  selector: 'app-overseer',
  templateUrl: './overseer.component.html',
  styleUrl: './overseer.component.scss'
})
export class OverseerComponent implements OnInit{

  public members: {userId: string, fullName: string}[] = [];
  public userSelected: boolean = false;

  private readonly _userService: UserService = inject(UserService);
  private readonly _dialog: MatDialog = inject(MatDialog);

  ngOnInit(): void {
    this.getMembers();
  }

  private getMembers(): void {
    this._userService.getUsers().subscribe({
      next: ((response: User[]): void => {
        if (response) {
          response.forEach((user: User): void => {
            const member: {userId: string, fullName: string} = {userId: user.id, fullName: `${user.firstName} ${user.lastName}`};
            if (member.fullName !== 'System Administrator')
            this.members.push(member);
          })
        }
      })
    });
  }

  onMemberChange(event: any): void {
    const userId: string | undefined = event.target.value;
    this.userSelected = !!userId;
  }

  onSendMail(): void {
    this._dialog.open(AdminSendEmailComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      disableClose: true,
      data: {memberEmails: this.members}
    })
  }
}
