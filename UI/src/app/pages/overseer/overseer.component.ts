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
import {OverseerValidationResultComponent} from "./overseer-validation-result/overseer-validation-result.component";

@Component({
  selector: 'app-overseer',
  templateUrl: './overseer.component.html',
  styleUrl: './overseer.component.scss'
})
export class OverseerComponent implements OnInit {

  // Variables to hold component data
  public members: { userId: string, fullName: string }[] = [];
  public userSelected: boolean = false;
  public yearlyDetails: OverseerCatchDetailsYearModel | undefined;
  public memberDetail: OverseerMemberDetailsModel | undefined;
  public currentYear: number = new Date().getFullYear();
  public availableYears: number[] = [];
  public selectedYear: number = this.currentYear;
  public selectedUserId: string = '';

  // Dependency injection
  private readonly _userService: UserService = inject(UserService);
  private readonly _overseerService: OverseerService = inject(OverseerService);
  private readonly _dialog: MatDialog = inject(MatDialog);
  private readonly _toastr: ToastrService = inject(ToastrService);

  ngOnInit(): void {
    // Fetch members
    this.getMembers();
    // Fetch yearly details for the current year
    this.getYearlyDetails(this.currentYear);
    // Populate available years array with the current year and previous two years
    for (let i: number = 0; i < 3; i++) {
      this.availableYears.push(this.currentYear - i);
    }
  }

  // Method to handle year change
  onYearChange(event: any): void {
    this.selectedYear = event.target.value;
    this.selectedUserId = '';
    this.userSelected = false;
    // Fetch yearly details for the selected year
    this.getYearlyDetails(this.selectedYear);
  }

  // Method to handle member change
  onMemberChange(event: any): void {
    const userId: string | undefined = event.target.value;
    if (userId) {
      this.selectedUserId = userId;
      this.userSelected = true;
      // Fetch member details for the selected user id
      this.getMemberDetails(userId);
    } else {
      this.userSelected = false;
      this.selectedUserId = '';
    }
  }

  // Method to open dialog for sending email
  onSendMail(): void {
    this._dialog.open(AdminSendEmailComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      disableClose: true,
      data: {memberEmails: this.members}
    })
  }

  // Method to download yearly excel report
  onDownloadExcel(): void {
    this._overseerService.getYearlyExcelReport(this.selectedYear).subscribe({
      next: ((response: { image: Blob, filename: string | null }): void => {
        this.downloadExcel(response);
        this._toastr.success('Jahres Statistik wird heruntergeladen', 'Jahres Statistik');
      }),
      // Handling errors and displaying toastr messages
      error: (): void => {
        this._toastr.error('Jahres Statistik kann nicht heruntergeladen werden', 'Jahres Statistik');
      }
    });
  }

  // Method to download member-specific yearly excel report
  onDownloadMemberExcel(): void {
    this._overseerService.getYearlyMemberExcelReport(this.selectedYear, this.selectedUserId).subscribe({
      next: ((response: { image: Blob, filename: string | null }): void => {
        this.downloadExcel(response);
        this._toastr.success('Mitglieder Statistik wird heruntergeladen', 'Mitglieder Statistik');
      }),
      // Handling errors and displaying toastr messages
      error: (): void => {
        this._toastr.error('Mitglieder Statistik kann nicht heruntergeladen werden', 'Mitglieder Statistik');
      }
    })
  }

  // Method to open dialog for validating license
  onValidateLicense(): void {
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

  // Method to fetch members
  private getMembers(): void {
    this._userService.getUsers().subscribe({
      next: ((response: User[]): void => {
        if (response) {
          response.forEach((user: User): void => {
            const member: { userId: string, fullName: string } = {
              userId: user.id,
              fullName: `${user.firstName} ${user.lastName}`
            };
            if (member.fullName !== 'System Administrator')
              this.members.push(member);
          })
        }
      })
    });
  }

  // Method to fetch yearly catch details
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

  // Method to fetch member details for a specific year
  private getMemberDetails(userId: string): void {
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

  // Method to download excel file
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

  // Method to validate license
  private validateLicense(licenseId: string): void {
    this._overseerService.validateLicense(licenseId).subscribe({
      next: ((response: OverseerMemberDetailsModel): void => {
        if (response) {
          this._dialog.open(OverseerValidationResultComponent, {
            width: '80%',
            height: 'auto',
            data: {validation: response}
          })
        }
      }),
      error: (): void => {
        this._toastr.error('E-Patent kann nicht validiert werden', 'E-Patent Validierung');
      }
    });
  }
}
