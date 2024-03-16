import {Component, inject, OnInit} from '@angular/core';
import {UserService} from "../../services/user.service";
import {User} from "../../models/user.model";
import {MatDialog} from "@angular/material/dialog";
import {AdminSendEmailComponent} from "../admin/admin-users/admin-send-email/admin-send-email.component";
import {OverseerService} from "../../services/overseer.service";
import {OverseerCatchDetailsYearModel} from "../../models/overseerCatchDetailsYear.model";
import {OverseerMemberDetailsModel} from "../../models/overseerMemberDetails.model";
import {ToastrService} from "ngx-toastr";
import {OverseerValidateLicenseComponent} from "./overseer-validate-license/overseer-validate-license.component";
import {FishingLicenseService} from "../../services/fishing-license.service";
import {FishingLicense} from "../../models/fishingLicense.model";
import {OverseerValidationResultComponent} from "./overseer-validation-result/overseer-validation-result.component";

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
  public availableYears: number[] = [];
  public selectedYear: number = this.currentYear;
  public selectedUserId: string = '';

  private readonly _userService: UserService = inject(UserService);
  private readonly _overseerService: OverseerService = inject(OverseerService);
  private readonly _dialog: MatDialog = inject(MatDialog);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _licenseService: FishingLicenseService = inject(FishingLicenseService);

  ngOnInit(): void {
    this.getMembers();
    this.getYearlyDetails(this.currentYear);
    for (let i = 0; i < 3; i++) {
      this.availableYears.push(this.currentYear - i);
    }
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
    this._overseerService.getMemberDetails(userId, this.selectedYear).subscribe({
      next: ((response: OverseerMemberDetailsModel): void => {
        if (response) {
          this.memberDetail = response;
        }
      }),
      error: ((error): void => {
        console.log(error);
        this.userSelected = false;
        this._toastr.error(`Mitglied hat keine Lizenz für ${this.selectedYear}`, 'Keine Lizenz');
      })
    });
  }

  onYearChange(event: any): void {
    this.selectedYear = event.target.value;
    this.selectedUserId = '';
    this.userSelected = false;
    this.getYearlyDetails(this.selectedYear);
  }

  onMemberChange(event: any): void {
    const userId: string | undefined = event.target.value;
    if (userId) {
      this.selectedUserId = userId;
      this.userSelected = true;
      this.getMemberDetails(userId);
    } else  {
      this.userSelected = false;
      this.selectedUserId = '';
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

  onDownloadExcel(): void {
    this._overseerService.getYearlyExcelReport(this.selectedYear).subscribe({
      next: ((response: {image: Blob, filename: string | null}): void => {
        this.downloadExcel(response);
        this._toastr.success('Jahres Statistik wird heruntergeladen', 'Jahres Statistik');
      }),
      // Handling errors and displaying toastr messages
      error: (): void =>{
        this._toastr.error('Jahres Statistik kann nicht heruntergeladen werden', 'Jahres Statistik');
      }
    });
  }


  onDownloadMemberExcel(): void {
    this._overseerService.getYearlyMemberExcelReport(this.selectedYear, this.selectedUserId).subscribe({
      next: ((response: {image: Blob, filename: string | null}): void => {
        this.downloadExcel(response);
        this._toastr.success('Mitglieder Statistik wird heruntergeladen', 'Mitglieder Statistik');
      }),
      // Handling errors and displaying toastr messages
      error: (): void =>{
        this._toastr.error('Mitglieder Statistik kann nicht heruntergeladen werden', 'Mitglieder Statistik');
      }
    })
  }

  private downloadExcel({filename, image}: { image: Blob, filename: string | null }): void {
    const fileUrl: string = URL.createObjectURL(image);
    const anchorElement: HTMLAnchorElement = document.createElement('a');
    anchorElement.href = fileUrl;
    anchorElement.target = '_blank';
    if (typeof filename === "string") {
      anchorElement.download = filename;
    }
    document.body.appendChild(anchorElement);
    anchorElement.click();
  }

  onValidateLicense() {
    const dialogRef = this._dialog.open(OverseerValidateLicenseComponent, {
      width: '80%',
      height: 'auto'
    });

    dialogRef.afterClosed().subscribe({
      next: ((licenseId: string | null) => {
        if (licenseId) {
          this.validateLicense(licenseId);
        }
      })
    })
  }

  private validateLicense(licenseId: string) {
    this._overseerService.validateLicense(licenseId).subscribe({
      next: ((response) => {
        if (response) {
          this._dialog.open(OverseerValidationResultComponent, {
            width: '80%',
            height: 'auto',
            data: {validation: response}
          })
        }
      }),
      error: () => {
        this._toastr.error('E-Patent kann nicht validiert werden', 'E-Patent Validierung');
      }
    });
  }
}
