import { ref, Ref } from 'vue';

interface UploadableFile {
  file: File;
  id: string;
  url: string;
  status: 'loading' | boolean | null;
  date: Date;
}

class UploadableFileImpl implements UploadableFile {
  file: File;
  id: string;
  url: string;
  status: 'loading' | boolean | null;
  date: Date;

  constructor(file: File) {
    this.file = file;
    this.id = `${file.name}-${file.size}-${new Date().toISOString()}-${file.type}`;
    this.url = URL.createObjectURL(file);
    this.status = null;
    this.date = new Date();
  }
}

export default function useFileList() {
  const files: Ref<UploadableFile[]> = ref([]);

  function addFiles(newFiles: File[]) {
    let newUploadableFiles = [...newFiles]
      .map((file) => new UploadableFileImpl(file))
      .filter((file) => !fileExists(file.id));
    files.value = files.value.concat(newUploadableFiles);
  }

  function fileExists(otherId: string): boolean {
    return files.value.some(({ id }) => id === otherId);
  }

  function removeFile(file: UploadableFile) {
    const index = files.value.indexOf(file);
    if (index > -1) files.value.splice(index, 1);
  }

  return { files, addFiles, removeFile };
}
