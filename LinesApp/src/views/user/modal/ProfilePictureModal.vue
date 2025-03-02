<script setup lang="ts">
import { computed, ref } from 'vue';
import { FwbButton } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { useMeStore } from '@/stores/data/settings/users/me';
// @ts-ignore
import VueCropper from 'vue-cropperjs';
import 'cropperjs/dist/cropper.css';

// Props
defineProps<{ imageUrl: string | null }>();

// Stores
const meStore = useMeStore();

// Refs
const modalRef = ref<InstanceType<typeof Modal>>();
const cropperRef = ref<InstanceType<any> | null>(null);

const isMobile = computed(() => {
  return 'ontouchstart' in window || navigator.maxTouchPoints > 0;
});


// Methods
const closeModal = () => {
  modalRef.value?.setModalOpen(false);
};

const confirmUpload = () => {
  if (!cropperRef.value) return;

  try {
    const croppedCanvas = cropperRef.value.cropper.getCroppedCanvas();
    if (!croppedCanvas) throw new Error("Failed to get cropped canvas");

    // Convert canvas to Blob
    croppedCanvas.toBlob(async (blob: any) => {
      if (!blob) throw new Error("Failed to convert canvas to Blob");
      const avatar = new File([blob], "avatar.png", { type: "image/png" });
      await meStore.modifyMyAvatar({ avatar });

      closeModal();

      console.log("Cropped Image uploaded successfully!");
    }, "image/png");
  } catch (error) {
    console.error("Error cropping and uploading image:", error);
  }
};

// Expose API
defineExpose({
  setModalOpen: (value: boolean) => {
    modalRef.value?.setModalOpen(value);
  },
});
</script>

<template>
  <Modal ref="modalRef" size="xl" set_margins>
    <!-- Header -->
    <template #header>
      <h3 class="text-base lg:text-xl font-semibold text-gray-900">Crop your new profile picture</h3>
    </template>

    <!-- Body -->
    <template #body>
      <div class="flex flex-col gap-3 p-1">
        <vue-cropper 
          v-if="imageUrl" 
          ref="cropperRef" 
          :src="imageUrl" 
          alt="Source Image" 
          :aspect-ratio="1" 
          :view-mode="1"
          :drag-mode="'move'" 
          :auto-crop-area="1" 
          :background="false" 
          :rotatable="true"
        />
        <!-- Help text for zooming -->
        <p class="text-sm text-gray-500 text-center">
          {{ isMobile ? 'Pinch to zoom in or out on the image.' : 'Use the mouse scroll wheel to zoom in or out on the image.' }}
        </p>
      </div>
    </template>

    <!-- Footer -->
    <template #footer>
      <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill>Cancel</fwb-button>
        <fwb-button @click="confirmUpload" class="bg-primary-600 hover:bg-brand-600" pill>
          Set new profile picture
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
<style scoped>
vue-cropper {
  max-width: 100%;
  height: 5000px;
}
:deep(.cropper-view-box),
:deep(.cropper-face) {
  border-radius: 50% !important;
}
</style>
