package jp.gr.java_conf.schkit.utils;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URL;
import java.net.URLClassLoader;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.security.ProtectionDomain;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.jar.Attributes;
import java.util.jar.Manifest;

import org.apache.commons.lang3.StringUtils;

public class ApplicationInfo {
    private static final String MANIFEST_PATH = "META-INF/MANIFEST.MF";

    private static URL classLotation;
    private static URLClassLoader classLoader;

    private String name = "";
    private String version = "";
    private String author = "";
    private String license = "";
    private String modified = "";

    private Path workingDir = null;
    private Path executeDir = null;

    private static final ApplicationInfo instance = new ApplicationInfo();

    private ApplicationInfo() {
        try {
            ProtectionDomain domain = this.getClass().getProtectionDomain();
            classLotation = domain.getCodeSource().getLocation();
            classLoader = (URLClassLoader) domain.getClassLoader();

            loadManifest();
            // setDirectories();
        } catch (Exception e) {
        }
    }

    private void loadManifest() throws Exception {
        if (classLoader == null)
            return;

        URL resource = classLoader.findResource(MANIFEST_PATH);
        if (resource == null)
            return;

        Manifest manifest = new Manifest(resource.openStream());

        Attributes attrs = manifest.getMainAttributes();

        String value;
        value = attrs.getValue("Implementation-Title");
        if (value != null)
            name = value;
        value = attrs.getValue("Implementation-Version");
        if (value != null)
            version = value;
        value = attrs.getValue("Create-By");
        if (value != null)
            author = value;
        value = attrs.getValue("License-By");
        if (value != null)
            license = value;

        if (classLotation != null) {
            Date date = new Date(classLotation.openConnection().getLastModified());
            SimpleDateFormat sdf = new SimpleDateFormat("yyyy.MM.dd HH:mm:ss");
            modified = sdf.format(date);
        }
    }

    private void setDirectories() throws Exception {
        // get execute directory
        if (classLotation != null)
            executeDir = Paths.get(classLotation.toURI());

        // get/set working directory
        String tmpdir = System.getProperty("java.io.tmpdir");
        if (tmpdir == null)
            return;

        Path dir = Paths.get(tmpdir);
        if (!Files.exists(dir))
            return;

        String tmpname = createWorkingDirectoryName();
        workingDir = Paths.get(dir.toAbsolutePath().toString(), tmpname);
        if (!Files.exists(workingDir)) {
            Files.createDirectories(workingDir);
        }

    }

    private String createWorkingDirectoryName() {
        String pathname = "";
        if (!StringUtils.isBlank(name) && !StringUtils.isBlank(version)) {
            pathname = String.format("%s-%s", name, version);
        } else {
            SimpleDateFormat sdf = new SimpleDateFormat("yyyy.MM.dd HH-mm-ss");
            pathname = "tmp-" + sdf.format(new Date());
        }
        pathname = pathname.replaceAll("[^a-zA-Z0-9.-]", "_");
        return pathname;
    }

    public static String name() {
        return instance.name;
    }

    public static String version() {
        return instance.version;
    }

    public static String author() {
        return instance.author;
    }

    public static String license() {
        return instance.license;
    }

    public static String modified() {
        return instance.modified;
    }

    public static Path workingDir() {
        return instance.workingDir;
    }

    public static Path executeDir() {
        return instance.executeDir;
    }

    /**
     * リソース取得
     *
     * @param e.g.
     *            xslt/iso_abstract_expand.xsl
     * @return
     */
    public static URL getResource(String name) {
        return classLoader != null ? classLoader.getResource(name) : null;
    }

    public static InputStream getResourceStream(String name) {
        //        return classLoader != null ? classLoader.getResourceAsStream(name) : null;
        return classLoader != null ? classLoader.getSystemResourceAsStream(name) : null;
    }

    public static String ResourceAsString(InputStream is) {

        StringBuilder sb = new StringBuilder();
        try {
            BufferedReader br = new BufferedReader(
                    new InputStreamReader(is));

            String line;

            while ((line = br.readLine()) != null) {
                sb.append(line);
            }

            br.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return sb.toString();
    }
}