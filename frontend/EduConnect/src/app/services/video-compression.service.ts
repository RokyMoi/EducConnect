import { Injectable } from '@angular/core';
import { SnackboxService } from './shared/snackbox.service';
import { FFmpeg } from '@ffmpeg/ffmpeg';
import { fetchFile } from '@ffmpeg/util';

export interface VideoCompressionOptions {
  crf?: number;
  preset?: string;
  outputFormat?: 'mp4' | 'webm';
}

@Injectable({
  providedIn: 'root',
})
export class VideoCompressionService {
  private ffmpeg = new FFmpeg();

  constructor(private snackboxService: SnackboxService) {
    this.loadFFmpeg();
  }
  private async loadFFmpeg() {
    if (!this.ffmpeg.loaded) {
      this.ffmpeg.load();
    }
  }

  public async compressVideo(
    file: File,
    options?: { bitrate?: string; resolution?: string; crf?: number }
  ): Promise<File> {
    await this.loadFFmpeg();

    const inputFileName = 'input.mp4';
    const outputFileName = 'output.mp4';

    const fileData = await fetchFile(file);
    await this.ffmpeg.writeFile(inputFileName, fileData);

    const bitrate = options?.bitrate || '800k';
    const resolution = options?.resolution || '1280x720';

    await this.ffmpeg.exec([
      '-i',
      inputFileName,
      '-c:v',
      'libx264',
      '-crf',
      options?.crf?.toString() || '23',
      '-b:v',
      bitrate,
      '-s',
      resolution,
      '-preset', 'veryfast',
      outputFileName,
    ]);

    const outputData = await this.ffmpeg.readFile(outputFileName);
    const compressedBlob = new Blob([outputData], { type: 'video/mp4' });

    await this.ffmpeg.deleteFile(inputFileName);
    await this.ffmpeg.deleteFile(outputFileName);

    var compressedFile = new File([compressedBlob], file.name, {
      type: 'video/mp4',
    });

    return compressedFile;
  }
}
