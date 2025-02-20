<script setup lang="ts">
import { FwbButton } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { ref, computed } from 'vue';
import { Dropdown, DropdownMenu, DropdownItem } from '@/components/dropdown/index';
import {
  IconDotHorizontal,
  IconEdit05,
  IconPlusCircle,
  IconUploadCloud01,
  IconPlus,
  IconTrash01,
} from '@/components/icons/index';
import DropZone from '@/components/dropzone/DropZone.vue';
import FilePreview from '@/components/dropzone/FilePreview.vue';
import useFileList from '@/compositions/file-list.ts';
import { useModalStore } from '@/stores/modal';
import { Encounter, Job, JobStatus } from '@/api/__generated__/graphql';
import { useEncountersStore } from '@/stores/data/encounters';

interface UploadableFile {
  file: File;
  id: string;
  url: string;
  status: 'loading' | boolean | null;
  date: Date;
}

const props = defineProps<{
  encounter: Encounter;
  job: Job;
}>();

const emit = defineEmits<{
  (e: 'width', val: string): void;
}>();

const modalStore = useModalStore();
const encountersStore = useEncountersStore();

const allowEdit = computed(() => props.job.status !== JobStatus.Canceled);

const { files, addFiles, removeFile } = useFileList();
const isUploading = ref(false);

function onInputChange(e: Event) {
  const input = e.target as HTMLInputElement;
  if (input.files) {
    const fileArray = Array.from(input.files);
    addFiles(fileArray);
    input.value = '';
  }
}

// Uploader

async function uploadFiles(files: UploadableFile[]) {
  isUploading.value = true;
  try {

    await encountersStore.attachPhotosToEncounter({
      encounterId: props.encounter.id,
      photos: files.map(f => f.file as File),
    });

    files.length = 0;
    modalDrawerRef.value?.setModalOpen(false);
  } catch (error) {
    console.error('Error uploading files:', error);
  } finally {
    isUploading.value = false;
  }
}



const modalDrawerRef = ref<InstanceType<typeof ModalDrawer>>();

const formatDate = (date: Date): string => {
  const options: Intl.DateTimeFormatOptions = {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
    hour: 'numeric',
    minute: 'numeric',
    hour12: true,
  };
  return new Intl.DateTimeFormat('en-US', options).format(date);
};

const photosGroupedByDate = computed(() => {
  if (!props.encounter || !props.encounter.photos) return [];
  const grouped = props.encounter.photos.reduce((acc: Record<string, any>, photo) => {
    const dateStr = formatDate(new Date(photo?.createdAt));
    if (!acc[dateStr]) {
      acc[dateStr] = { date: dateStr, photos: [] };
    }
    acc[dateStr].photos.push(photo);
    return acc;
  }, {});
  return Object.values(grouped);
});

// Edit mode state
const isEditMode = ref(false);
const editGroupDate = ref<string | null>(null);

function toggleEditMode(date: string) {
  isEditMode.value = true;
  editGroupDate.value = isEditMode.value ? date : null;
}

function deletePhoto(photoId: string) {
  encountersStore.removePhotoFromEncounter({
    encounterId: props.encounter.id,
    id: photoId,
  });
}

function handleClickAddPhotos() {
  modalDrawerRef.value?.setModalOpen(true);
  if (!modalStore.isJobModalExpended) {
    emit('width', 'full');
    modalStore.isJobModalExpended = true;
    modalStore.isJobModalAutoExpended = true;
  }
}

function photoModalClosed() {
  // reset job modal width if auto expended
  if (modalStore.isJobModalAutoExpended) {
    emit('width', '7xl');
    modalStore.isJobModalExpended = false;
    modalStore.isJobModalAutoExpended = false;
  }
}
</script>
<template>
  <div>
    <!-- Photos grouped by date -->
    <div
      v-if="photosGroupedByDate.length"
      class="flex flex-col gap-4 pb-8 lg:pb-8 mb-3 lg:mb-4 border-b border-slate-300"
    >
      <div v-for="group in photosGroupedByDate" :key="group.date" class="w-full">
        <div class="flex items-center justify-between w-full">
          <div class="text-xs font-medium text-slate-500">{{ group.date }}</div>
          <Dropdown align-to-end class="rounded-lg" close-inside>
            <template #trigger>
              <fwb-button color="light" pill square class="border-white">
                <IconDotHorizontal />
              </fwb-button>
            </template>
            <DropdownMenu class="!w-36 !min-w-36">
              <DropdownItem @click="toggleEditMode(group.date)">
                <IconEdit05 class="mr-2" />
                Edit
              </DropdownItem>
            </DropdownMenu>
          </Dropdown>
        </div>
        <div class="flex items-center flex-wrap gap-2 pt-2">
          <div v-for="photo in group.photos" :key="photo.id" class="relative">
            <img :src="photo.url" class="w-[122px] h-[122px] object-cover" />
            <div
              v-if="isEditMode && editGroupDate === group.date"
              class="absolute inset-0 flex items-center justify-center"
            >
              <fwb-button @click="deletePhoto(photo.id)" color="light" pill square
                ><IconTrash01
              /></fwb-button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Add Photos -->
    <div v-if="allowEdit">
      <fwb-button        
        @click="handleClickAddPhotos"
        color="light"
        pill
        class="font-semibold px-0 hover:bg-white focus:ring-0 border-0 text-brand-600"
      >
        <template #prefix>
          <IconPlusCircle />
        </template>
        Add photos
      </fwb-button>
    </div>

    <!-- Add Photos Modal -->
    <ModalDrawer
      ref="modalDrawerRef"
      no_footer
      max_width="lg"
      set_margins
      @close="photoModalClosed"
    >
      <template #header>
        <h3 class="text-base lg:text-xl font-semibold text-gray-900">Add Photos</h3>
      </template>
      <template #body>
        <div class="flex flex-col">
          <DropZone
            class="flex flex-col w-full"
            @files-dropped="addFiles"
            #default="{ dropZoneActive }"
          >
            <label
              for="file-input"
              class="flex flex-col items-center justify-center w-full h-64 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-gray-50 hover:bg-gray-100"
            >
              <div class="flex flex-col items-center justify-center pt-5 pb-6">
                <IconUploadCloud01 class="text-brand-700" />
                <span v-if="dropZoneActive" class="text-sm text-slate-500">
                  <span>Drop them here</span>
                </span>
                <span v-else class="text-sm text-slate-500">
                  <span class="font-semibold">Click to upload</span> or drag and drop
                </span>
                <span class="text-xs text-slate-500 mt-1 mb-4">Max. File Size: 30MB</span>
                <fwb-button class="pointer-events-none bg-primary-600 hover:bg-brand-600" pill>
                  <template #prefix> <IconPlus height="16" width="16" /> </template>
                  Add Images
                </fwb-button>
              </div>
              <input type="file" id="file-input" multiple @change="onInputChange" class="hidden" />
            </label>
            <ul class="image-list flex flex-wrap mt-4" v-show="files.length">
              <FilePreview
                v-for="file of files"
                :key="file.id"
                :file="file"
                tag="li"
                @remove="removeFile"
              />
            </ul>
          </DropZone>
          <fwb-button
            v-show="files.length"
            @click.prevent="uploadFiles(files)"
            :disabled="isUploading"
            class="bg-primary-600 hover:bg-brand-600 w-full mt-4"
            pill
          >
            Upload</fwb-button
          >
        </div>
      </template>
    </ModalDrawer>
  </div>
</template>

<style scoped>
.drop-area {
  width: 100%;
  max-width: 800px;
  margin: 0 auto;
  padding: 50px;
  background: #ffffff55;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
  transition: 0.2s ease;

  &[data-active='true'] {
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
    background: #ffffffcc;
  }
}
</style>
