export enum PromotionStatus {
  Draft = 0,
  Active = 1,
  Paused = 2,
  Completed = 3,
  Canceled = 4
}

export interface CoursePromotion {
  duration: string;
  promotionId: string;
  courseId: string;
  courseName: string;
  title: string;
  description: string;
  status: PromotionStatus;
  createdAt: number;
  updatedAt?: number;
  startDate?: number;
  endDate?: number;
  mainImage?: string;
}

export interface CoursePromotionDetail extends CoursePromotion {
  images: PromotionImage[];
}

export interface PromotionImage {
  imageId: string;
  displayOrder: number;
  isMainImage: boolean;
  fileName: string;
}

export interface CreatePromotionDto {
  courseId: string;
  title: string;
  description: string;
  startDate: number;
  endDate: number;
  images?: File[];
}

export interface UpdatePromotionDto {
  promotionId: string;
  title: string;
  description: string;
  status?: PromotionStatus;
  startDate?: number;
  endDate?: number;
  newImages?: File[];
  mainImageId?: string;
  removeImageIds?: string[];
}

export interface UpdateStatusDto {
  status: PromotionStatus;
}

export interface Course {
  id: string;
  title: string;
}
