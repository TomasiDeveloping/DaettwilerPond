import {Component, inject, OnInit} from '@angular/core';
import {UserService} from "../../services/user.service";
import {User} from "../../models/user.model";
import {MatDialog} from "@angular/material/dialog";
import {AdminSendEmailComponent} from "../admin/admin-users/admin-send-email/admin-send-email.component";
import {OverseerService} from "../../services/overseer.service";
import {OverseerCatchDetailsYearModel} from "../../models/overseerCatchDetailsYear.model";
import {OverseerMemberDetailsModel} from "../../models/overseerMemberDetails.model";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-overseer',
  templateUrl: './overseer.component.html',
  styleUrl: './overseer.component.scss'
})
export class OverseerComponent implements OnInit{

  public members: {userId: string, fullName: string}[] = [];
  public userSelected: boolean = false;
  public yearlyDetails: OverseerCatchDetailsYearModel | undefined;
  public memberDetail: OverseerMemberDetailsModel | undefined;
  public currentYear: number = new Date().getFullYear();

  private readonly _userService: UserService = inject(UserService);
  private readonly _overseerService: OverseerService = inject(OverseerService);
  private readonly _dialog: MatDialog = inject(MatDialog);
  private readonly _toastr: ToastrService = inject(ToastrService);

  ngOnInit(): void {
    this.getMembers();
    this.getYearlyDetails(this.currentYear);
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

  private getYearlyDetails(year: number): void {
    this._overseerService.getYearlyDetails(year).subscribe({
      next: ((response: OverseerCatchDetailsYearModel): void => {
        if (response) {
          this.yearlyDetails = response;
        }
      }),
      error: ((error): void => {
        console.log(error);
      })
    });
  }

  private getMemberDetails(userId: string): void{
    this._overseerService.getMemberDetails(userId).subscribe({
      next: ((response: OverseerMemberDetailsModel): void => {
        if (response) {
          this.memberDetail = response;
        }
      }),
      error: ((error): void => {
        console.log(error);
      })
    });
  }

  onMemberChange(event: any): void {
    const userId: string | undefined = event.target.value;
    if (userId) {
      this.userSelected = true;
      this.getMemberDetails(userId);
    } else  {
      this.userSelected = false;
    }
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

  onDownloadExcel() {
    this._overseerService.getYearlyExcelReport(this.currentYear).subscribe({
      next: ((response: {image: Blob, filename: string | null}): void => {
        const fileUrl: string = URL.createObjectURL(response.image);
        const anchorElement: HTMLAnchorElement = document.createElement('a');
        anchorElement.href = fileUrl;
        anchorElement.target = '_blank';
        if (typeof response.filename === "string") {
          anchorElement.download = response.filename;
        }
        document.body.appendChild(anchorElement);
        anchorElement.click();

        this._toastr.success('Statistik wird heruntergeladen', 'Statistik');
      }),
      // Handling errors and displaying toastr messages
      error: (): void =>{
        this._toastr.error('Statistik kann nicht heruntergeladen werden', 'Statistik');
      }
    });
  }
}
