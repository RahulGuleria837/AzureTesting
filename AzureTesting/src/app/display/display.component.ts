import { Update } from './../update';
import { Component } from '@angular/core';
import { DisplayService } from '../display.service';
import { Display } from '../display';

@Component({
  selector: 'app-display',
  templateUrl: './display.component.html',
  styleUrls: ['./display.component.scss']
})
export class DisplayComponent {
  displayList:any[]=[];
  editData: Display= new Display();
  

  constructor(private displayService:DisplayService){}

  ngOnInit():void{
    this.getAll();
  }
    getAll(){
      this.displayService.getAllData().subscribe(
        (response)=>{
          this.displayList=response;
          console.log(response);
        },
        (error)=>{
          console.log(error);
        }
      )}

      download(fileName:string,fileExtension:string) {
      
        debugger
        const file=fileName+"."+fileExtension;
        this.displayService.downloadImage(file).subscribe(blob => {
          const url = URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = url;
          link.download = file;
          link.click();
          URL.revokeObjectURL(url);
        });


      }

      editbutton(event:any,i:number){
        this.editData.fileName=this.displayList[i].fileName;
        this.editData.fileExtension=this.displayList[i].fileExtension;
      }

      editclick(){
    this.displayService.EditdataImage(this.editData).subscribe(
      (response)=>{
        alert('Edited succesfully')
      }
    )
      }
    }