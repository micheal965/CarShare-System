export const convertImage = (pic) => {
	const bytes = new Uint8Array(pic);
	const binaryString = String.fromCharCode(...bytes);
	return `data:image/jpeg;base64,${btoa(binaryString)}`;
};
